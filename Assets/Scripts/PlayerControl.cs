using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
	public GameObject lineContainer;
	public Tutor tutor;
	public Level level;
	public int lengthOfRope;
	public int balls;
	private int dragCount;
	private bool finished;
	private bool tutored;
	private Vector3 lastMouseCoordinate = Vector3.zero;
	private bool isMousePressed;
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
		Input.simulateMouseWithTouches = true;
	}
				
	// Update is called once per frame
	void Update () {
		if(!finished) {
			if (SystemInfo.deviceType == DeviceType.Desktop) {
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
							tutored = true;
						}
					}
				}
			lastMouseCoordinate = Input.mousePosition;
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
								
			} else {
				if (Input.touchCount > 0) {
					if (Input.GetTouch (0).phase == TouchPhase.Began) {
						GameObject l = (GameObject)Instantiate (line, transform.position, transform.rotation);
						l.transform.parent = lineContainer.transform;
						Lines.Add (l.GetComponent<Line> ());
					}

					if (Input.GetTouch (0).phase == TouchPhase.Moved) {
						mousePos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
						mousePos.z = 0;
						if (Lines.Count > 0) {
							if (!Lines [Lines.Count - 1].pointsList.Contains (mousePos)) {
								dragCount++;
								if (dragCount == 2) {
									GetComponent<AudioSource> ().Play ();				
								}
								Lines [Lines.Count - 1].addPoint (mousePos);
							}
						}

					}

					if (Input.GetTouch (0).phase == TouchPhase.Ended) {
						dragCount = 0;
					}

				}
			}
			int linesLength = 0;
			for (int i = 0; i < Lines.Count; i++) {
				linesLength += Lines [i].pointsList.Count;
			}

			if (linesLength > lengthOfRope) {
				if (Lines [0].Shorten ()) {
					Lines.RemoveAt (0);
				}
			}

		}
	}

	public void GameOver() {		
		finished = true;
		level.LevelFailPanel.SetActive (true);
		ProcGenMusic.MusicGenerator.Instance.Stop ();
	}
	
}
