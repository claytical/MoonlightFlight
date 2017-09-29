using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {
	public GameObject LevelClearText;
	private AsyncOperation AO;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ScanForCompletion() {

		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Disappearing");
		if (gos.Length > 0) {
			Debug.Log ("Still bumpable objects");

		} else {
			LevelClearText.SetActive (true);
			//TODO: Calculate Points
			StartCoroutine("backToLevelSelect");
		}
	}


	IEnumerator backToLevelSelect() {
		AO = SceneManager.LoadSceneAsync ("LevelSelect", LoadSceneMode.Single);
		AO.allowSceneActivation = false;
		while (AO.progress < 0.9f) {
			yield return null;
		}
		AO.allowSceneActivation = true;
	}

}
