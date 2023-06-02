using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class ManageCargo : MonoBehaviour
{
    public PrototypeVehicle prototype;
    public PowerUp.Reward selectedCargo;
    public Fleet fleet;
    public Text shipName;
    public CargoSupply[] supplies;

    // Start is called before the first frame update
    void Start()
    {
        prototype = fleet.vehiclesAvailable[DialogueLua.GetVariable("Vehicle Type").asInt];
        shipName.text = DialogueLua.GetVariable("Ship Name").asString;
        //Get this vehicle type's cargo capacity
        //prototype.cargoCapacity

        //count current stored cargo
        int nukesOnBoard = PlayerPrefs.GetInt(DialogueLua.GetVariable("Ship Name").asString + PowerUp.Reward.Nuke.ToString(), 0);
        int brakesOnBoard = PlayerPrefs.GetInt(DialogueLua.GetVariable("Ship Name").asString + PowerUp.Reward.Stop.ToString(), 0);
        int totalCargo = nukesOnBoard + brakesOnBoard;
        int slotIndex = 0;
        
        //insert nukes into cargo
        for(int i = 0; i < nukesOnBoard; i++)
        {
            CargoSlot slot = new CargoSlot();
            slot.cargo = PowerUp.Reward.Nuke;
            slot.inUse = true;
            slotIndex++;
        }

        //insert brakes into cargo
        for(int i = slotIndex; i < brakesOnBoard; i++)
        {
            CargoSlot slot = new CargoSlot();
            slot.cargo = PowerUp.Reward.Stop;
            slot.inUse = true;
            slotIndex++;

        }

    }

    public void SetCargo(PowerUp.Reward cargo)
    {
        selectedCargo = cargo;
    }
    // Update is called once per frame
    
    public void UpdateSupplyCount()
    {
        for(int i = 0; i < supplies.Length; i++)
        {
            supplies[i].SetCount();
        }
    }
}
