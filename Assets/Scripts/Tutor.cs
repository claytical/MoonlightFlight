using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutor : MonoBehaviour {

	public GameObject lines;
	public GameObject finger;
	public GameObject dropButton;
	public GameObject bumper;
	public GameObject[] script;

	private bool lineHasBeenDrawn;
	private bool buttonHasBeenPressed;

	void Start () {
		GetComponent<Animator> ().SetTrigger ("drawWithFinger");
		lineHasBeenDrawn = false;
		for (int i = 1; i < script.Length; i++) {
			script [i].SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!lineHasBeenDrawn) {
			if (lines.GetComponentInChildren<LineRenderer> () != null) {
				lineHasBeenDrawn = true;
				finger.SetActive (false);
				script [0].SetActive (false);
				script [1].SetActive (true);
				Debug.Log ("Line has been drawn");
				dropButton.GetComponent<Animator> ().SetTrigger ("wiggle");
			}
		}
		if (buttonHasBeenPressed) {
			dropButton.GetComponent<Animator> ().SetTrigger ("normal");
			script [1].SetActive (false);
			script [2].SetActive (true);
			buttonHasBeenPressed = false;
		}

		if (bumper == null) {
			Destroy (this.gameObject);
		}

	}

	public void pressedDropButton() {
		buttonHasBeenPressed = true;
	}

}
