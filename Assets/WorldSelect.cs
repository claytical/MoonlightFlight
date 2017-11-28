using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSelect : MonoBehaviour {
	public Button[] worlds;
	public bool allWorldsUnlocked;
	// Use this for initialization
	void Start () {
		if (allWorldsUnlocked) {
			for (int i = 0; i < worlds.Length; i++) {
				worlds [i].interactable = true;
			}
		}
		else {
			for (int i = 0; i < worlds.Length; i++) {
				if (PlayerPrefs.HasKey (worlds [i].gameObject.name)) {
					worlds [i].interactable = true;
				} else {
					worlds [i].interactable = false;
				}
			}
		}
	}
	
}
