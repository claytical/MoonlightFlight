using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour {
	public GameObject[] stars;

	public void Start() {
		if (Camera.main.GetComponent<AudioSource> () != null) {
			Camera.main.GetComponent<AudioSource> ().Stop ();

		}
	}

	public void SetStars(int amount) {
		for (int i = 0; i < amount; i++) {
			if (i < stars.Length) {
				stars [i].SetActive (true);
			}
		}
	}
}
