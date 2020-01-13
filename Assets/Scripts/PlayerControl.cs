using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
	public GameObject lineContainer;
	public Tutor tutor;
	public EndlessLevel level;
	public float amountOfInk;
	public int balls;
	private int dragCount;
	private bool finished;
	private bool tutored;
    private bool endOfInk = false;
	private Vector3 lastMouseCoordinate = Vector3.zero;
	private bool isMousePressed;
	public List<Line> Lines;
	public GameObject line;
    public float lineTimeLimit = 100f;
    public Text inkLeft;
    public float inkAmount = 0;
    public GameObject inkJar;
    public GameObject goButton;

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
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        finished = false;
		tutored = false;
		Input.simulateMouseWithTouches = true;
	}
				
	// Update is called once per frame
	void Update () {
		if(!finished) {
            //magic ink
            /*
            if (!endOfInk && level.inkEnabled)
            {
                if (SystemInfo.deviceType == DeviceType.Desktop)
                {
                    Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
                    if (Input.GetMouseButtonDown(0))
                    {
                        //start sound
                        isMousePressed = true;
                        GameObject l = (GameObject)Instantiate(line, transform.position, transform.rotation);
                        l.transform.parent = lineContainer.transform;
                        Lines.Add(l.GetComponent<Line>());
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        //stop sound
                        isMousePressed = false;
                        dragCount = 0;
                    }

                    if (Input.GetMouseButton(0))
                    {
                        if (mouseDelta.magnitude > 0)
                        {
                            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (tutor && !tutored)
                            {
                                tutored = true;
                            }
                        }
                    }
                    lastMouseCoordinate = Input.mousePosition;
                    if (isMousePressed)
                    {
                        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mousePos.z = 0;
                        if (Lines.Count > 0)
                        {
                            if (!Lines[Lines.Count - 1].pointsList.Contains(mousePos))
                            {
                                dragCount++;
                                if (dragCount == 2)
                                {
                                    GetComponent<AudioSource>().Play();
                                }
                                Lines[Lines.Count - 1].addPoint(mousePos);
                            }
                        }
                    }

                }
                else
                {
                    if (Input.touchCount > 0)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            GameObject l = (GameObject)Instantiate(line, transform.position, transform.rotation);
                            l.transform.parent = lineContainer.transform;
                            Lines.Add(l.GetComponent<Line>());
                        }

                        if (Input.GetTouch(0).phase == TouchPhase.Moved)
                        {
                            mousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                            mousePos.z = 0;
                            if (Lines.Count > 0)
                            {
                                if (!Lines[Lines.Count - 1].pointsList.Contains(mousePos))
                                {
                                    dragCount++;
                                    if (dragCount == 2)
                                    {
                                        GetComponent<AudioSource>().Play();
                                    }
                                    Lines[Lines.Count - 1].addPoint(mousePos);
                                }
                            }
                        }

                        if (Input.GetTouch(0).phase == TouchPhase.Ended)
                        {
                            dragCount = 0;
                        }

                    }
                }

                int linesLength = 0;
                for (int i = 0; i < Lines.Count; i++)
                {
                    linesLength += Lines[i].pointsList.Count;
                    //                    inkLeft.text = (linesLength - amountOfInk).ToString();
                }
                

                inkAmount = linesLength;
                if (linesLength > amountOfInk)
                {

                    endOfInk = true;
                    if(inkJar.activeSelf)
                    {
                        inkJar.SetActive(false);
                        goButton.SetActive(true);
//                        GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer.FindSnapshot("Normal").TransitionTo(0);
                    }
                    
                }
                float ink = (1 - (inkAmount / amountOfInk));
                if(ink < 0)
                {
                    ink = 0;
                }
                Debug.Log(inkAmount + " divided by " + amountOfInk + " equals " + ink);
                inkLeft.text = ink.ToString("0%");

            }
            */
		}


    }

    public void GameOver(string message) {		
		finished = true;
        level.grid.currentSet.Waiting();
        level.LevelFailPanel.SetActive (true);
        level.failureMessage.text = message;
        //		ProcGenMusic.MusicGenerator.Instance.Stop ();
        endOfInk = false;
    }
    public void Pause()
    {
        Time.timeScale = 0f;    
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
    }

}
