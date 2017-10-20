using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutor : MonoBehaviour {
	public GameObject[] steps;
	public GameObject finger;
	public PlayerControl player;
	public Button ballDrop;
	private int stepIndex;
//	private int currentStep;
	// Use this for initialization

	//#1 - Drag
	//#2 - Drop
	//#3 - Draw
	//#4 - Clear

	void Start () {
		if(PlayerPrefs.HasKey("tutored")) {
			gameObject.SetActive(false);
		}
		ballDrop.enabled = false;
		stepIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		

	public void Continue(int currentStep) {
		if(stepIndex == currentStep) {
			switch(currentStep) {
				case 0:
					//LEARNED TO DRAW
					finger.SetActive(false);
					steps[0].SetActive(false);
					steps[1].SetActive(true);	
					ballDrop.enabled = true;
					break;
				case 1:
					//LEARNED TO DROP BALL
					steps[1].SetActive(false);
					steps[2].SetActive(true);
					break;
				case 2:
					//LEARNED HOW TO CONTROL BALL MOVEMENT
					steps[2].SetActive(false);
					steps[3].SetActive(true);
					//ADD BUMPER
					player.level.tutorialBumper();
					break;
				case 3:
					//LEARNED HOW TO DESTROY BUMPER
					steps[3].SetActive(false);
					steps[4].SetActive(true);					
					PlayerPrefs.SetString("tutored", "very true");
					Debug.Log("Tutorial Over");
					Destroy(gameObject,3);
					break;
			}
			stepIndex++;
		}
	}
}
