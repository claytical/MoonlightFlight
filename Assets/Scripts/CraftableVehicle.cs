using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftableVehicle : MonoBehaviour
{

    public VehicleType vehicle;
    public int partsRequired;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>().text = partsRequired.ToString("0");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
