using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleUnlockPanel : MonoBehaviour
{
    public GameObject vehicleContainer;
    public VehicleStats[] vehicles;

    // Start is called before the first frame update
    void Start()
    {
        vehicles = vehicleContainer.GetComponentsInChildren<VehicleStats>();
        for(int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].gameObject.SetActive(false);
        }
        //turn off panel
        gameObject.SetActive(false);
    }

    public void UnlockVehicle(VehicleType type)
    {
        for(int i = 0; i < vehicles.Length; i++)
        {
            if(vehicles[i].type == type)
            {
                Debug.Log("Unlocking " + type);
                vehicles[i].gameObject.SetActive(true);
                break;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
