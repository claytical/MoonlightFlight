using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
	public GameObject peg;
	public GameObject pegContainer;
	public GameObject lineContainer;
	public GameObject gameOverText;
	public Transform ballHolder;
	public Tutor tutor;
	public LevelGenerator level;
	public int numberOfPegs;
	public int balls;
	private int dragCount;
	private bool finished;
	private bool tutored;
	private bool readyForStep3;
	private Vector3 lastMouseCoordinate = Vector3.zero;
//	private LineRenderer line;
	private bool isMousePressed;
	public List<Vector3> pointsList;
	public List<Line> Lines;
	public GameObject line;

	private Vector3 mousePos;

	struct myLine {
		public Vector3 StartPoint;
		public Vector3 EndPoint;
	};

	void Awake () {
		isMousePressed = false;
		Lines = new List<Line>();
	}

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
				isMousePressed = true;
				GameObject l = (GameObject) Instantiate(line, transform.position, transform.rotation);
				l.transform.parent = lineContainer.transform;
				Lines.Add(l.GetComponent<Line>());

			}

			if(Input.GetMouseButtonUp(0)) {
				//stop sound
				isMousePressed = false;
				dragCount = 0;
			}
			if(Input.GetMouseButton(0)) {
				if(mouseDelta.magnitude > 0) {
					//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if(tutor && !tutored) {
						tutor.Continue(0);
						tutored = true;
					}
				}
			}

			lastMouseCoordinate = Input.mousePosition;
			int linesLength = 0;
			for(int i = 0; i < Lines.Count; i++) {
				linesLength += Lines[i].pointsList.Count;
			}

			if(linesLength > numberOfPegs) {
				if(Lines[0].Shorten()) {
					Lines.RemoveAt(0);
				}
			}

			if (isMousePressed) {
				mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				mousePos.z = 0;
				if(Lines.Count > 0) {
					if(!Lines[Lines.Count - 1].pointsList.Contains(mousePos)) {
						dragCount++;
						if(dragCount == 2) {
							GetComponent<AudioSource>().Play();				
						}
						Lines[Lines.Count - 1].addPoint(mousePos);
					}
				}
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
