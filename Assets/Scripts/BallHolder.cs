using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    // Use this for initialization

    void Start () {
        //		setBallDisplay ();
        multiplier = 1;
        maxFever = false;
        /*
        inkJar.SetActive(player.level.inkEnabled);
        */
	}

    public int increaseMultiplier()
    {
//        bool speedUp = false;
        int speedState = 0;
        if(multiplier < 27)
        {
            multiplier++;
            feverBar.increaseFever();
            speedState = 1;
        }
        if(multiplier >= 27 && !maxFever)
        {
            //EVENT #3 - FEVER REACHED
            speedState = 2;
        }
        return speedState;
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

    // Update is called once per frame
    void Update()
    {


        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
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
