using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class GameState : MonoBehaviour {


    public string SelectedWorld;
	public string SelectedLevel;
    public bool resetKeys = false;
    private ShipType selectedShip;
    public VehicleType selectedVehicle;
    //    public Ship ship;


    public void SetShip(ShipType _shipType)
    {
        selectedShip = _shipType;
    }

    public ShipType GetShip()
    {
        return selectedShip;
    }


    public void SetVehicle(VehicleType _vehicleType)
    {
        selectedVehicle = _vehicleType;
    }

    public VehicleType GetVehicle()
    {
        return selectedVehicle;
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
        if(resetKeys)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt(selectedVehicle.ToString(), 0);

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
