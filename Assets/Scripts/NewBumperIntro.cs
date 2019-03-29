using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBumperIntro : MonoBehaviour {
	public GameObject bumper;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (bumper == null) {
			GetComponent<Animator> ().SetTrigger ("fade");
		}
		
	}

	public void fadeComplete() {
		Destroy (this.gameObject);
	}
}

