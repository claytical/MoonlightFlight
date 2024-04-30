using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int playerId;
    private int vehicleId;
    public GameObject chosenVehicle;
    public GameObject playerUI;

    private PlayerStats playerStats;
    private Vehicle vehicle;
    private bool thrust = false;
    public float transitionDuration = 1f;
    private ParkingLot parkingLot;

    private void Start()
    {
        //FIND POSSIBLE VEHICLES
        parkingLot = FindObjectOfType<ParkingLot>();
        playerId = PlayerInputManager.instance.playerCount;
        if(playerUI.GetComponent<PlayerStats>()) {
            GameObject go = Instantiate(playerUI, PlayerInputManager.instance.gameObject.transform);
            playerStats = go.GetComponent<PlayerStats>();
            playerStats.GetComponentInParent<Players>().PlayerJoined(playerId);
        }
        else
        {
            Debug.Log("This prefab requires a PlayerStats component.");
        }
        AssignFirstVehicleInParkingLot();
    }


    public void Restart()
    {
        playerStats.Deactivate();
        SwitchActionMap("Start");
        
    }

    public void GlitchScanLines()
    {
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().scanLineJitter = 1f;
        Invoke("ResetCamera", .1f);
    }

    public void GlitchColorDrift()
    {
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift = .2f;
        Invoke("ResetCamera", .2f);
    }

    public void GlitchVerticalJump()
    {
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = .05f;

        Invoke("ResetCamera", .2f);
    }

    public void ResetCamera()
    {
        Debug.Log("Resetting Glitch");
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().scanLineJitter = 0f;
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift = 0f;
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = 0f;

    }

    public bool TakeDamage(int damage)
    {
        return(playerStats.TakeDamage(damage));
    }

    public void IncreaseHP()
    { 
        playerStats.hp.IncreaseHP(1);
    }

    public void IncreaseFuel()
    {
        playerStats.fuel.Recharge();
        playerStats.fuel.UpdateUI();
    }
    
    public void DecreaseFuel()
    {
        playerStats.fuel.UpdateUI();
    }
    public void EnergyCollected()
    {
        playerStats.EnergyCollected(1);
    }

    public void SetVehicleIconColor(Color color)
    {
        playerStats.vehicleIcon.color = color;
    }

    private void AssignFirstVehicleInParkingLot()
    {

        if (parkingLot != null)
        {
            vehicleId = parkingLot.FirstAvailableVehicle();
       
            chosenVehicle = parkingLot.vehicles[vehicleId];

            playerStats.vehicleIcon.sprite = parkingLot.vehicles[vehicleId].GetComponent<SpriteRenderer>().sprite;
        }

    }

    public void NextVehicle(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            vehicleId = parkingLot.NextAvailableVehicle(vehicleId);
            chosenVehicle = parkingLot.vehicles[vehicleId];
            playerStats.vehicleIcon.sprite = parkingLot.vehicles[vehicleId].GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void PreviousVehicle(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            vehicleId = parkingLot.PreviousAvailableVehicle(vehicleId);
            chosenVehicle = parkingLot.vehicles[vehicleId];
            playerStats.vehicleIcon.sprite = parkingLot.vehicles[vehicleId].GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void SwitchActionMap(string map)
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap(map);
    }

    public void ChooseVehicle(InputAction.CallbackContext value)
    {

        if (value.started)
        {
            if (chosenVehicle)
            {
                Transform[] spawnLocations = playerStats.GetComponentInParent<Players>().spawnLocations.GetComponentsInChildren<Transform>();

                if (playerStats.GetComponentInParent<Players>())
                {
                    playerStats.GetComponentInParent<Players>().PlayerChoseVehicle(playerId);
                }
                SwitchActionMap("Play");
                chosenVehicle = Instantiate(chosenVehicle,transform);
                if (chosenVehicle.GetComponent<Vehicle>())
                {
                    vehicle = chosenVehicle.GetComponent<Vehicle>();
                }

                if (spawnLocations[playerId])
                {
                    chosenVehicle.transform.position = spawnLocations[playerId].position;
                }
                vehicle.Fly();
                playerStats.SetStats(vehicle);

            }
            else
            {
                Debug.Log("No available vehicles!");
            }

        }

    }

    private void TurnOffTrails()
    {
        vehicle.TurnOffBoost();

    }


    public void Move(InputAction.CallbackContext value)
    {
        vehicle.GetComponent<Vehicle>().Move(value.ReadValue<Vector2>().normalized);
    }

    public void Thrust(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Debug.Log("Thrust Started");
            if(!vehicle.isBoosting())
            {
                playerStats.fuel.Boost();
                if (vehicle)
                {
                    vehicle.TurnOnBoost();
                    playerStats.fuel.UpdateUI();
                }

            }
        }


    }



}
