using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {
	public GameObject peg;
	public GameObject pegContainer;
	public GameObject gameOverText;
	public Transform ballHolder;
	public Tutor tutor;
	public LevelGenerator level;
	public int numberOfPegs;
	public int balls;
	private bool finished;
	private bool tutored;
	private bool readyForStep3;
	private Vector3 lastMouseCoordinate = Vector3.zero;

	// Use this for initialization
	void Start () {
		finished = false;
		tutored = false;
		readyForStep3 = false;
	}

	public void setReadyForStep3() {
		readyForStep3 = true;
	}

	public void Retry() {
		balls = 5;
		finished = false;
		ballHolder.gameObject.GetComponent<BallHolder>().Reset();
//		level.ClearGrid();
//		level.Generate();
		for (int i = 0; i < pegContainer.transform.childCount; i++) {
			Destroy(pegContainer.transform.GetChild(i).gameObject);
		}
	}
		
	// Update is called once per frame
	void Update () {
		if(ballHolder.childCount <= 0 && balls <= 0 && !finished) {
			GameOver();
		}
		if(!finished) {
			Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
			if(Input.GetMouseButtonDown(0)) {
				//start sound
			}
			if(Input.GetMouseButtonUp(0)) {
				//stop sound
			}
			if(Input.GetMouseButton(0)) {
				if(mouseDelta.magnitude > 0) {
					//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if(tutor.gameObject.activeSelf && !tutored) {
						tutor.Continue();
						tutored = true;
					}
					if(readyForStep3) {
						tutor.Continue();
						readyForStep3 = false;
					}
					Vector3 finger = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					//			if (Physics.Raycast(ray)) {
					finger.z = 10;
					GameObject ball = (GameObject) Instantiate(peg, finger, transform.rotation);
					ball.transform.parent = pegContainer.transform;
					GetComponent<AudioSource>().Play();
					}
				}

			lastMouseCoordinate = Input.mousePosition;
//			pegContainer.GetComponentsInChildren<Transform>()


			if(pegContainer.transform.childCount > numberOfPegs) {
					Destroy(pegContainer.transform.GetChild(0).gameObject);
			}
		}
	}

	public void GameOver() {		
		finished = true;
		//turn on game over component
		gameOverText.SetActive(true);
		gameOverText.GetComponent<GameOver>().levelReachedLabel.text = "You reached level " + level.currentLevel + "!";
	}
	
}
