using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BallHolder : MonoBehaviour {
	public GameObject ball;
	public GameObject holder;
	public PlayerControl player;
	public Button button;
    public Fever feverBar;
    public GameObject inkJar;
    public GameObject textOverlay;
    public GameObject uiCanvas;
    public int maxFeverPlaytime;
    public int multiplier;
    private bool maxFever;
    private float maxFeverTime;
	private int score;
    public Grid grid;
    public GameObject touchPoint;
    private List<GameObject> touchPoints;
    private bool[] startedTouching;

    // Use this for initialization

    void Start () {
        //		setBallDisplay ();
        multiplier = 1;
        maxFever = false;
        touchPoints = new List<GameObject>();
        /*
        inkJar.SetActive(player.level.inkEnabled);
        */
    }

    public int increaseMultiplier(int amount)
    {
        //        bool speedUp = false;
        if (amount > 0)
        {
            int speedState = 0;
            if (multiplier < 26)
            {
                multiplier+=amount;
                feverBar.increaseFever(amount);
                speedState = 1;
            }
            else if(!maxFever) { 
                //EVENT #3 - FEVER REACHED
                speedState = 2;
            }
            return speedState;
        }
        return -1;
    }

    public void FeverReached()
    {
        //        Instantiate(textOverlay, uiCanvas.transform);
        grid.currentSet.BroadcastMessage("FeverReached", SendMessageOptions.DontRequireReceiver);
        maxFever = true;
        maxFeverTime = Time.time + maxFeverPlaytime;

    }

    public void BreakFever()
    {
        multiplier = 1;
        maxFever = false;
        //        GetComponentInParent<LevelSound>().NormalMode();
        Debug.Log("Breaking Fever");
        ball.GetComponent<Animator>().SetTrigger("fever");
        feverBar.resetFever();
        ball.GetComponent<Ball>().force = 5f;

    }

    public bool HasFever()
    {
        return maxFever;
    }


    void CheckTouches()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if(touch.phase == TouchPhase.Began)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                GameObject go = (GameObject) Instantiate(touchPoint, touch.position, transform.rotation);
                touchPosition.z = 0;
                touchPoints.Add(go);
            }
            if(touch.phase == TouchPhase.Ended)
            {
                GameObject tP = touchPoints[i];
                touchPoints.RemoveAt(i);
                Destroy(tP,5);
            }
            if(touch.phase == TouchPhase.Moved)
            {
                touchPoints[i].transform.position = touch.position;
            }
        }


    }
    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
            GameObject go = (GameObject)Instantiate(touchPoint, touchPosition, transform.rotation);
            go.transform.parent = transform;
           
            touchPoints.Add(go);
        }
        if(Input.GetMouseButtonUp(0))
        {
            GameObject tP = touchPoints[0];
            touchPoints.RemoveAt(0);
            Destroy(tP,5);
        }

        if (Input.GetMouseButton(0)) {
            
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (ball)
                {
                    Vector2 direction = (Vector2)touchPosition - (Vector2)ball.transform.position;
                    direction.Normalize();

                    ball.GetComponent<Rigidbody2D>().AddForce(direction * 20f, ForceMode2D.Impulse);
                }
                /*            touchPosition.z = ball.transform.position.z - Camera.main.transform.position.z;
                            touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                            touchPosition.y = ball.transform.position.y;
                            */

                //            Vector2 dir = lastKnown - (Vector2)(player.transform.position);
            }
            else
            {
                for(int i = 0; i < Input.touchCount; i++)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    if (ball)
                    {
                        Vector2 direction = (Vector2)touchPosition - (Vector2)ball.transform.position;
                        direction.Normalize();

                        ball.GetComponent<Rigidbody2D>().AddForce(direction * 20f, ForceMode2D.Impulse);
                    }

                }
            }

        if (maxFever && maxFeverTime <= Time.time) {
            //EVENT #6 - FEVER TIME OUT
            BreakFever();
            Debug.Log("Fever Timeout");
            grid.currentSet.BroadcastMessage("FeverTimeout", SendMessageOptions.DontRequireReceiver);
        }
	}

	public void DeadBall() {
        //		button.interactable = true;
        player.GameOver("Your firefly died...");
    }

	public void Drop() {
        Time.timeScale = 1f;
        button.GetComponent<Animator>().SetTrigger("pop");
        ball.GetComponent<Ball>().inPlay = true;
        if(GetComponentInParent<LevelSound>())
        {
//            GetComponentInParent<LevelSound>().NormalMode();

        }
        grid.currentSet.Starting();

        /*
        chute.SetTrigger ("fire");
		button.interactable = false;
		if(player.tutor) {
			player.tutor.GetComponent<Tutor> ().pressedDropButton ();
			GameObject b = (GameObject) Instantiate(ball, transform.position, transform.rotation);
			b.transform.parent = transform;

		}
		else {
			if(player.balls > 0) {
				GameObject b = (GameObject) Instantiate(ball, transform.position, transform.rotation);
				b.transform.parent = transform;
				player.balls--;
			}	
		}
		setBallDisplay ();
        */
    }
    /*
	private void setBallDisplay() {
		for (int i = 0; i < ballDisplay.Length; i++) {
			if (i < player.balls) {
				ballDisplay [i].SetActive (true);
			} else {
				ballDisplay [i].SetActive (false);
			}
		}

	}
    */
    public void removePoints() {
		score = 0;
	}
	public void addPoints(int points) {
        Debug.Log("Adding " + points + " points");
		score += points;
	}

	public void Reset() {
		score = 0;
	}


}
