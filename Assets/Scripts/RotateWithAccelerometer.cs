using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithAccelerometer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, -Input.acceleration.x * 1.0f));
	}
}
