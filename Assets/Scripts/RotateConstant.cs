using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstant : MonoBehaviour {
    public float acceleration;
    public Vector3 accelerate;
    public bool randomizeRotation;
	// Use this for initialization
	void Start () {
		if(randomizeRotation)
        {
            accelerate = new Vector3(Random.value, Random.value, Random.value);
        }
	}

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(accelerate);
    }
}
