using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	public string SelectedWorld;
	public string SelectedLevel;

	void Awake() {
		GameState[] gameStates = FindObjectsOfType (typeof(GameState)) as GameState[];	
		if (gameStates.Length > 1) {
			Debug.Log ("Getting rid of excess game states");
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad (gameObject);

		}
	}
	// Use this for initialization
	void Start () {
//        GameTune.Initialize("026bab3d-3490-4a7f-beba-60c6947e88f2");
    }
	
    public void UseTiltControls()
    {
        PlayerPrefs.SetInt("tilt", 1);
    }

    public void UseTouchControls()
    {
        PlayerPrefs.SetInt("tilt", 0);

    }

	// Update is called once per frame
	void Update () {
		
	}
}
