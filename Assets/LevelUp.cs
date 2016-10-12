using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour {
	public LevelGenerator levelGenerator;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeLevel() {
		GetComponent<Text>().text = "LEVEL " + levelGenerator.currentLevel; 
	}

	public void resetLabel() {
		GetComponent<Text>().text = "CLEARED!";
	}

	public void hide() {
		levelGenerator.Generate();
		gameObject.SetActive(false);

	}
}
