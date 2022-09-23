using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePowerUps : MonoBehaviour
{
    public ParkingLot lot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        lot.GetVehicle().ApplyHyperBreak();
    }

    public void SetNukes()
    {

    }

    public void SetBrakes()
    {

    }
}
