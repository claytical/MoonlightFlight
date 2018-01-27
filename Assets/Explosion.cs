using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	// Use this for initialization

	void Start () {
		Rigidbody2D[] splinters = GetComponentsInChildren<Rigidbody2D> ();
		for(int i = 0; i < splinters.Length; i++) {
			Vector2 force =	new Vector2 (Random.Range (-25, 25), Random.Range (-25, 25));
			splinters [i].AddForce (force, ForceMode2D.Impulse);
		}

	}	
}
