using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class Crafting : MonoBehaviour
{
    public Text partsAvailable;
    public Text shipNameInput;
    public GameObject vehicleToCraft;
    public CraftableVehicle[] allVehicles;
    private int vehicleIndex = 0;
    public Button craftButton;
    public GameObject SelectShipSlot;
    public Fleet fleet;

    // Start is called before the first frame update
    void Start()
    {
        int parts = DialogueLua.GetVariable("Parts").asInt;
        partsAvailable.text = parts.ToString("0");
        ShowCraftableVehicle();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextCraftableVehicle()
    {
        vehicleIndex++;
        if(vehicleIndex == allVehicles.Length)
        {
            vehicleIndex = 0;
        }
        ShowCraftableVehicle();
    }

    public void PreviousCraftableVehicle()
    {
        vehicleIndex--;
        if (vehicleIndex < 0)
        {
            vehicleIndex = allVehicles.Length - 1;
        }
        ShowCraftableVehicle();
    }

    private void ShowCraftableVehicle()
    {
        if (vehicleToCraft)
        {
            Destroy(vehicleToCraft);
        }

        vehicleToCraft = Instantiate(allVehicles[vehicleIndex].gameObject, SelectShipSlot.transform);
        if(System.Int32.Parse(partsAvailable.text) < vehicleToCraft.GetComponent<CraftableVehicle>().partsRequired)
        {
            craftButton.interactable = false;
        }
        else
        {
            craftButton.interactable = true;
        }
    }


    public void CraftVehicle()
    {
        fleet.CreateShip(vehicleToCraft.GetComponent<CraftableVehicle>(), shipNameInput.text);

        }

}
