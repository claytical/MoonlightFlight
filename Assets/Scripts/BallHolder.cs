using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallHolder : MonoBehaviour {
	public GameObject ball;
	public GameObject holder;
//	public GameObject[] ballDisplay;
	public PlayerControl player;
//	public Animator chute;
	public Button button;
    public Fever feverBar;
    public GameObject inkJar;
    public GameObject textOverlay;
    public GameObject uiCanvas;
    public int multiplier;
    private bool maxFever;
	private int score;

	// Use this for initialization
	void Start () {
        //		setBallDisplay ();
        multiplier = 1;
        maxFever = false;
        inkJar.SetActive(player.level.inkEnabled);

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
        if(multiplier == 27 && !maxFever)
        {
            speedState = 2;
            maxFever = true;
            Debug.Log("MAX FEVER!!!");
            Instantiate(textOverlay, uiCanvas.transform);
        }
        return speedState;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DeadBall() {
        //		button.interactable = true;
        player.GameOver();
    }

	public void Drop() {
        Time.timeScale = 1f;
        button.GetComponent<Animator>().SetTrigger("pop");
        ball.GetComponent<Ball>().inPlay = true;
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
