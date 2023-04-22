using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class PrototypeVehicle : MonoBehaviour
{
    public VehicleType vehicle;
    public CargoHold nukes;
    public CargoHold brakes;
    public int nukesInStorage;
    public int brakesInStorage;
    public int cargoCapacity = 8;
    
    // Start is called before the first frame update
    void Start()
    {
        //ship amounts
        nukes.amount = PlayerPrefs.GetInt(DialogueLua.GetVariable("Ship Name").asString + PowerUp.Reward.Nuke.ToString(), 0);
        brakes.amount = PlayerPrefs.GetInt(DialogueLua.GetVariable("Ship Name").asString + PowerUp.Reward.Stop.ToString(), 0);

        //cold storage amounts

        nukesInStorage = PlayerPrefs.GetInt("nukes in storage", 0);
        brakesInStorage = PlayerPrefs.GetInt("brakes in storage", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
