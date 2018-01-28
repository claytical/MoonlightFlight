using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutor : MonoBehaviour {

	public GameObject lines;
	private bool lineHasBeenDrawn;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!lineHasBeenDrawn) {
			if (lines.GetComponentInChildren<LineRenderer> () != null) {
				lineHasBeenDrawn = true;
				Debug.Log ("Line has been drawn");
			}
		}
	}
		

}
