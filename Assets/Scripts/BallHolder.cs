using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallHolder : MonoBehaviour {
	public GameObject ball;
	public GameObject holder;
	public GameObject[] ballDisplay;
	public PlayerControl player;
	public Animator chute;
	public Button button;
	private int score;

	// Use this for initialization
	void Start () {
		setBallDisplay ();
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
	}

	private void setBallDisplay() {
		for (int i = 0; i < ballDisplay.Length; i++) {
			if (i < player.balls) {
				ballDisplay [i].SetActive (true);
			} else {
				ballDisplay [i].SetActive (false);
			}
		}

	}
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
