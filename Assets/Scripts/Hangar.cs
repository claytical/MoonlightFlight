using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Hangar : MonoBehaviour
{

    public GameObject[] planes;
    public GameObject setupScreen;
    private bool[] planeInUse;
 


    // Start is called before the first frame update
    void Start()
    {
        planeInUse = new bool[planes.Length];
    }

    public void OnJoin(PlayerInput playerInput)
    {
        //players++;
    }

    public int FirstAvailablePlane()
    {
        for (int i = 0; i < planes.Length; i++)
        {
            if (!planeInUse[i])
            {
                planeInUse[i] = true;
                return i;
            }
        }
        return -1;
    }

    public int NextAvailablePlane(int vehicleId)
    {
        vehicleId++;
        if (vehicleId >= planes.Length)
        {
            vehicleId = 0;
        }
        return vehicleId;
    }

    public int PreviousAvailableVehicle(int vehicleId)
    {
        vehicleId--;
        if (vehicleId < 0)
        {
            vehicleId = planes.Length - 1;
        }
        return vehicleId;
    }


}
