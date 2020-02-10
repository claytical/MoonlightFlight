using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    [System.Serializable]

    public enum Ship
    {
        Boomerang = 0,
        Rocket = 1,
        XWing = 2,
        Falcon = 3,
        Shooter = 4,
        UFO = 5
    };

    public string SelectedWorld;
	public string SelectedLevel;
    public Ship ship;


    public void ChooseShip(int s)
    {
        switch(s)
        {
            case 0:
                ship = Ship.Boomerang;
                break;
            case 1:
                ship = Ship.Rocket;
                break;
            case 2:
                ship = Ship.XWing;
                break;
            case 3:
                ship = Ship.Falcon;
                break;
            case 4:
                ship = Ship.Shooter;
                break;
            case 5:
                ship = Ship.UFO;
                break;
        }
    }

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
