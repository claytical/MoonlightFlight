using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fleet : MonoBehaviour
{

    public Text slotsAvailable;
    public GameObject vehicleToCraft;
    public List<PrototypeVehicle> vehiclesAvailable;
    public GameObject emptySlot;
    private int vehicleIndex = 0;
    public int availableSlots;
    public Button cargoButton;
    public Button salvageButton;
    public Button createButton;
    public Text currentShip;
    public GameObject Slots;
    private int selectedSlotIndex = 0;
    private string[] fleetNameArray;
    private int[] fleetTypeArray;
    // Start is called before the first frame update
    void Start()
    {
        fleetTypeArray = PlayerPrefsX.GetIntArray("Fleet Ship Types");
        fleetNameArray = PlayerPrefsX.GetStringArray("Fleet Ship Names");

        PopulateFleet();   
    }
    public void PopulateFleet()
    {
        slotsAvailable.text = PlayerPrefs.GetInt("parts", 0).ToString();
        int totalShips = PlayerPrefs.GetInt("Ships", 0);
        for (int i = 0; i < availableSlots; i++)
        {
            GameObject ship;

            if (i < totalShips)
            {
                ship = Instantiate(vehiclesAvailable[fleetTypeArray[i]].gameObject, Slots.transform);
            }
            else
            {
                //create empty slot
                ship = Instantiate(emptySlot, Slots.transform);
            }
            
            ship.SetActive(false);

        }
        DisplaySlot(selectedSlotIndex);

    }

    public void DisplaySlot(int slotNumber)
    {
        Image[] slots = Slots.GetComponentsInChildren<Image>(true);
        Debug.Log("Slots: " + slots.Length);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
  
        if(slots[slotNumber].GetComponent<PrototypeVehicle>())
        {
            //has a vehicle
            DisplayShipName(fleetNameArray[selectedSlotIndex]);
            salvageButton.gameObject.SetActive(true);
            cargoButton.gameObject.SetActive(true);
            createButton.gameObject.SetActive(false);
            slots[slotNumber].GetComponentInChildren<Text>().enabled = false;

        }
        else
        {
            //empty
            DisplayShipName("Empty");
            salvageButton.gameObject.SetActive(false);
            cargoButton.gameObject.SetActive(false);
            createButton.gameObject.SetActive(true);

        }

        slots[slotNumber].gameObject.SetActive(true);
//        PrototypeVehicle pVehicle = SelectShipSlot.GetComponentsInChildren<GameObject>()[slotNumber];
//        pVehicle.gameObject.SetActive(true);

    }

    public void NextSlot()
    {
        selectedSlotIndex++;
        if(selectedSlotIndex == availableSlots)
        {
            selectedSlotIndex = 0;
        }
        DisplaySlot(selectedSlotIndex);
    }

    public void PreviousSlot()
    {
        selectedSlotIndex--;
        if(selectedSlotIndex < 0)
        {
            selectedSlotIndex = availableSlots - 1;
        }
        DisplaySlot(selectedSlotIndex);

    }

    public void NextCraftableVehicle()
    {
        vehicleIndex++;
        if (vehicleIndex == vehiclesAvailable.Count)
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
            vehicleIndex = vehiclesAvailable.Count - 1;
        }
        ShowCraftableVehicle();
    }

    private void ShowCraftableVehicle()
    {
        if (vehicleToCraft)
        {
            Destroy(vehicleToCraft);
        }
        vehicleToCraft = Instantiate(vehiclesAvailable[vehicleIndex].gameObject, Slots.transform);
    }

    public void DisplayShipName(string shipName)
    {
        currentShip.text = shipName;
    }
}
