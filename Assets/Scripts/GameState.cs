using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {


    public string SelectedWorld;
	public string SelectedLevel;
    public bool resetKeys = false;
    private ShipType selectedShip;
    //    public Ship ship;


    public void SetShip(ShipType _shipType)
    {
        selectedShip = _shipType;
    }

    public ShipType GetShip()
    {
        return selectedShip;
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
//        GooglePlayGames.PlayGamesPlatform.Activate();
//        AppLovin.InitializeSdk();
        AppLovin.SetSdkKey("3C26Gk_Rq5H7BNZEyR9ggo3qKOxnKwdShfHMNDfMZKaAgfHaB92EGduEy6M1L4aCW5oSATrVLcGXQU-hw7Lfm2");
        AppLovin.InitializeSdk();
        if (resetKeys)
        {
            PlayerPrefs.DeleteAll();
        }
    }
	

    public void Login()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Login Sucess");
                }
                else
                {
                    Debug.Log("Login failed");
                }
            });

        }

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
