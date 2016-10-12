using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutor : MonoBehaviour {
	public GameObject[] steps;
	public GameObject finger;
	public PlayerControl player;
	private int currentStep;
	// Use this for initialization

	//#1 - Drag
	//#2 - Drop
	//#3 - Draw
	//#4 - Clear

	void Start () {
		if(PlayerPrefs.HasKey("tutored")) {
			gameObject.SetActive(false);
		}
		currentStep = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Drop() {
		if(currentStep == 1) {
			Continue();
			player.setReadyForStep3();
		}
	}


	public void Continue() {
		steps[currentStep].SetActive(false);
		if (currentStep == 0) {
			finger.SetActive(false);
		}
		currentStep++;
		if (currentStep >= steps.Length) {
			PlayerPrefs.SetString("tutored", "very true");
			Debug.Log("Tutorial Over");
			gameObject.SetActive(false);
		}
		else {
			steps[currentStep].SetActive(true);
		}
	}
}
