using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstant : MonoBehaviour {
    public float acceleration;
    public Vector3 accelerate;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(accelerate);
    }
}
