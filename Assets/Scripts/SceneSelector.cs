using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour {
	public string scene;
    AsyncOperation AO;
	private GameState gameState;

	// Use this for initialization
	void Start () {
		gameState = (GameState)FindObjectOfType (typeof(GameState));	

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
		if (gameState != null) {
			if (gameState.SelectedLevel != null) {

			}
			if (gameState.SelectedWorld != null) {

			}
		}
			
	}

	public void SetWorld(string world) {
		if (gameState != null) {
			gameState.SelectedWorld = world;
		} else {
			Debug.Log ("Game State Not Available");
		}
	}
		
	public void SetLevel(string level) {
		if (gameState != null) {
			gameState.SelectedLevel = level;
		} else {
			Debug.Log ("Game State Not Available");
		}
	}

	public void Previous() {
		if (scene == "LevelSelect") {
			//On World Scene, Go to Main Menu
			scene = "Main";
		}
		if (scene == "WorldSelect") {
			//On Level Scene, Go to World Select
			scene = "WorldSelect";
		}

		StartCoroutine ("load");

	}

}
