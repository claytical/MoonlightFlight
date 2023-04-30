using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
	public EndlessLevel level;
	private bool finished;
	private Vector3 lastMouseCoordinate = Vector3.zero;
    public GameObject boundary;

	private Vector3 mousePos;

	struct myLine {
		public Vector3 StartPoint;
		public Vector3 EndPoint;
	};

	void Awake () {
	}

	// Use this for initialization
	void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        finished = false;
		Input.simulateMouseWithTouches = true;
	}
				
	// Update is called once per frame
	void Update () {

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
