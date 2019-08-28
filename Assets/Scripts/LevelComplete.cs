using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour {
	public GameObject[] stars;
    public Text score;
	public void Start() {
		if (Camera.main.GetComponent<AudioSource> () != null) {
			Camera.main.GetComponent<AudioSource> ().Stop ();

		}
	}

    public void SetScore(int amount, int total)
    {
        score.text = amount.ToString("0") + total.ToString("/0 points");
    }

	public void SetStars(int amount) {
		for (int i = 0; i < amount; i++) {
			if (i < stars.Length) {
				stars [i].SetActive (true);
			}
		}
	}
}
