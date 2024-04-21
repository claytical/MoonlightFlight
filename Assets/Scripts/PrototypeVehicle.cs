using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeVehicle : MonoBehaviour
{
    public VehicleType vehicle;
    public int nukesInStorage;
    public int brakesInStorage;
    public int cargoCapacity = 8;
    
    // Start is called before the first frame update
    void Start()
    {
        //cold storage amounts
        nukesInStorage = PlayerPrefs.GetInt("nukes in storage", 0);
        brakesInStorage = PlayerPrefs.GetInt("brakes in storage", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
