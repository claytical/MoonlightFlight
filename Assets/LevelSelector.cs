using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {
	public string scene;
	AsyncOperation AO;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	IEnumerator load() {
		AO = SceneManager.LoadSceneAsync (scene, LoadSceneMode.Single);

		AO.allowSceneActivation = false;
		while (AO.progress < 0.9f) {
			yield return null;
		}
		AO.allowSceneActivation = true;
	}

	public void choose() {
		StartCoroutine ("load");
	}
}
