using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {
	public string scene;
	public bool locked;
	public GameObject lockImage;
	public GameObject levelName;
	private GameState gameState;
	AsyncOperation AO;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetLocks() {
		lockImage.SetActive (locked);
		levelName.SetActive (!locked);
		GetComponent<Button> ().interactable = !locked;

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
		gameState = (GameState)FindObjectOfType (typeof(GameState));	
		if (gameState != null) {
			gameState.SelectedLevel = levelName.GetComponent<Text> ().text;
		}
		StartCoroutine ("load");
	}
}
