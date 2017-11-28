using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallHolder : MonoBehaviour {
	public GameObject ball;
	public GameObject holder;
	public PlayerControl player;
	public Animator chute;
	public Text scoreUI;
	public Button button;
	private int score;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DeadBall() {
		button.interactable = true;
	}

	public void Drop() {
		chute.SetTrigger ("fire");
		button.interactable = false;
		if(player.tutor) {
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
	}

	public void removePoints() {
		score = 0;
		scoreUI.text = score.ToString ();
	}
	public void addPoints(int points) {
		score += points;
		scoreUI.text = score.ToString();
	}

	public void Reset() {
		score = 0;
		scoreUI.text = score.ToString();
	}


}
