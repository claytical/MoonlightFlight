using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ParkingLot : MonoBehaviour
{

    public GameObject[] vehicles;
    public bool[] vehicleInUse;
    public GameObject vehicle;
    public int energyCollected;
    public float lightYearsTraveled;
    public Damage HP;
    public UsePowerUps PowerUps;
    private int players = 0;

    // Start is called before the first frame update
    void Start()
    {
        vehicleInUse = new bool[vehicles.Length];
    }

    public void OnJoin(PlayerInput playerInput)
    {
        players++;
    }

    public int FirstAvailableVehicle()
    {
    for(int i = 0; i < vehicles.Length; i++)
        {
            if(!vehicleInUse[i])
            {
                vehicleInUse[i] = true;
                return i;
            }
        }
        return -1;
    }

    public int NextAvailableVehicle(int vehicleId)
    {
        vehicleId++;
        if(vehicleId >= vehicles.Length)
        {
            vehicleId = 0;
        }
        return vehicleId;
    }

    public int PreviousAvailableVehicle(int vehicleId)
    {
        Debug.Log("VEHICLE ID:  " + vehicle);
        vehicleId--;
        if (vehicleId < 0)
        {
            vehicleId = vehicles.Length - 1;
        }
        return vehicleId;
    }

    public void EnergyCollected()
    {
        energyCollected++;
    }


    public Vehicle DefaultVehicle()
    {
        vehicle = Instantiate(vehicles[0], transform);
        Vector3 newPosition = vehicle.transform.position;
        newPosition.z = 10f;
        vehicle.transform.position = newPosition;
        HP.SetHP(vehicle.GetComponentInChildren<Vehicle>());

        return vehicle.GetComponentInChildren<Vehicle>();
    }

}
