using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour {
    private Rigidbody2D body;
    private Vector3 velocity;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        velocity = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
    }
	
	// Update is called once per frame
	void Update () {
        body.AddForce(velocity);
        Debug.Log("Adding Force of : " + velocity);

    }
}
