using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UDP;
using GleyGameServices;
using UnityEngine.Events;
using Unity.Services.Core;
using Unity.Services.Analytics;
using PixelCrushers.DialogueSystem;

public class InitListener : IInitListener
{
    public void OnInitialized(UserInfo userInfo)
    {
        Debug.Log("Initialization succeeded");
        // You can call the QueryInventory method here
        // to check whether there are purchases that haven’t been consumed.       
    }

    public void OnInitializeFailed(string message)
    {
        Debug.Log("Initialization failed: " + message);
    }
}

public class InitWithDefault : MonoBehaviour
{
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }
            }
}

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

    public int GetVehicleTypeID ()
    {
        return ((int)selectedVehicle);
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
        if (resetKeys)
        {
            PlayerPrefs.DeleteAll();
//            DialogueManager.ResetDatabase(DatabaseResetOptions.KeepAllLoaded); // Reset state to initial values.
        }
/*        IInitListener listener = new InitListener();
        StoreService.Initialize(listener);
*/
    }

    public void UseGameServices()
    {
        if (!GameServices.Instance.IsLoggedIn())
        {
            GameServices.Instance.LogIn(LoginResult);
        }
        else
        {
            GameServices.Instance.ShowAchievementsUI();
        }

    }

    private void LoginResult(bool success)
    {
        if(success == true)
        {
            Debug.Log("Login Successful");
        }
        else
        {
            Debug.Log("Login failed");
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
