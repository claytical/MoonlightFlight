using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (-10, 10), Random.Range (-10, 10)), ForceMode2D.Force);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
