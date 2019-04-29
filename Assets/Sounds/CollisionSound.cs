using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour {
	public AudioClip[] soundFx;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
//		GetComponent<AudioSource> ().PlayOneShot (soundFx [Random.Range (0, soundFx.Length)]);
	}
}
