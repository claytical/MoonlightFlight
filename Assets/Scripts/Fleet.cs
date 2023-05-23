using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System.Linq;

public class Fleet : MonoBehaviour
{

    public List<PrototypeVehicle> vehiclesAvailable;
    public GameObject Slots;
    public GameObject emptySlot;
    public int availableSlots;

    public Crafting crafting;

    public Text slotNumber;
    public Button cargoButton;
    public Button salvageButton;
    public Button createButton;
    public Button flyButton;

    public Text currentShip;

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

    //ACTION FUNCTIONS

    public void GotoOutpost()
    {
        DialogueManager.PlaySequence("LoadLevel(Outpost)");
    }

    public void SalvageShip()
    {
        FleetLog();
        bool needNewShip = false;
        List<int> fleetTypes = fleetTypeArray.ToList();
        List<string> fleetNames = fleetNameArray.ToList();
        int totalShips = DialogueLua.GetVariable("Ships").asInt;
        Debug.Log("Salvaging " + currentShip.text + " a " + vehiclesAvailable[fleetTypeArray[selectedSlotIndex]].name + " vessel.");
        int parts = DialogueLua.GetVariable("Parts").asInt;
        int shipTypeBeingSalvaged = fleetTypes[selectedSlotIndex];
        CraftableVehicle salvagedShip = crafting.allVehicles[shipTypeBeingSalvaged];
         
        if(fleetNameArray.Length >= selectedSlotIndex)
        {
            if (DialogueLua.GetVariable("Ship Name").asString == fleetNames[selectedSlotIndex])
            {
                //current ship is being salvaged
                needNewShip = true;
            }

            //            vehiclesAvailable[fleetTypeArray[i]]


            fleetNames.RemoveAt(selectedSlotIndex);
            fleetTypes.RemoveAt(selectedSlotIndex);
            totalShips--;
            parts += salvagedShip.partsRequired;
            PlayerPrefsX.SetStringArray("Fleet Ship Names", fleetNames.ToArray());
            PlayerPrefsX.SetIntArray("Fleet Ship Types", fleetTypes.ToArray());
            fleetNameArray = fleetNames.ToArray();
            fleetTypeArray = fleetTypes.ToArray();

            if(needNewShip)
            {
                if(fleetNames.Count >= 0)
                {
                    //no ships!
                }
                else
                {
                    UseShip(0);
                }
            }
        }
        DialogueLua.SetVariable("Parts", parts);
        DialogueLua.SetVariable("Ships", totalShips);
        PlayerPrefs.SetInt("Ships", totalShips);
        FleetLog();
        string s = PersistentDataManager.GetSaveData(); // Save state.


    }
    public void UseShip(int selectedShip)
    {
        if(fleetNameArray.Length >= selectedShip)
        {
            DialogueLua.SetVariable("Ship Name", fleetNameArray[selectedShip]);
            DialogueLua.SetVariable("Selected Ship", selectedShip);
            DialogueLua.SetVariable("Vehicle Type", fleetTypeArray[selectedShip]);
        }
        else
        {
            Debug.Log("Ship ID doesn't exist");
        }

        string s = PersistentDataManager.GetSaveData(); // Save state.

    }

    public void Fly()
    {
        UseShip(selectedSlotIndex);
        DialogueManager.PlaySequence("LoadLevel(Remix)");
    }

    public void CreateShip(CraftableVehicle vehicle, string shipName)
    {
        FleetLog();

        //        int parts = DialogueLua.GetVariable("parts").asInt;
        int parts = 999;
        if(parts - vehicle.partsRequired >= 0)
        {
            parts = parts - vehicle.partsRequired;
            PlayerPrefs.SetInt("parts", parts);
            DialogueLua.SetVariable("parts", parts);

            List<int> fleetTypes = fleetTypeArray.ToList();
            List<string> fleetNames = fleetNameArray.ToList();
            int totalShips = fleetNameArray.Length;
            totalShips++;
            DialogueLua.SetVariable("Ships", totalShips);
            PlayerPrefs.SetInt("Ships", totalShips);

            DialogueLua.SetVariable("Vehicle Type", ((int)vehicle.vehicle));
            DialogueLua.SetVariable("Ship Name", shipName);

            fleetTypes.Add(DialogueLua.GetVariable("Vehicle Type").asInt);
            fleetNames.Add(DialogueLua.GetVariable("Ship Name").asString);

            PlayerPrefsX.SetIntArray("Fleet Ship Types", fleetTypes.ToArray());
            PlayerPrefsX.SetStringArray("Fleet Ship Names", fleetNames.ToArray());
            fleetNameArray = fleetNames.ToArray();
            fleetTypeArray = fleetTypes.ToArray();

            PlayerPrefs.SetInt(DialogueLua.GetVariable("Ship Name").asString + PowerUp.Reward.Nuke.ToString(), 0);
            PlayerPrefs.SetInt(DialogueLua.GetVariable("Ship Name").asString + PowerUp.Reward.Stop.ToString(), 0);


            Debug.Log("Ran Create Ship with ID " + fleetTypes[totalShips - 1] + " named " + shipName);
            DialogueLua.SetVariable("Ships", fleetNames.Count);
            PlayerPrefs.SetInt("Ships", fleetNames.Count);
            DialogueLua.SetVariable("Ship Name", shipName);
            selectedSlotIndex = totalShips - 1;
            DisplaySlot(totalShips - 1);
            string s = PersistentDataManager.GetSaveData(); // Save state.



        }
        else
        {
            Debug.Log("Not enough parts!");
        }



    }

    public void ClearChildren(Transform t)
    {
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[t.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in t)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now destroy them
        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }

    }
    public void FleetLog()
    {
        Debug.Log("FLEET SIZE:" + fleetNameArray.Length);
        for(int i = 0; i < fleetTypeArray.Length; i++)
        {
            Debug.Log(i + ": " + fleetNameArray[i] + ", SHIP TYPE " + fleetTypeArray[i]);
        }

    }
    //DISPLAY FUNCTIONS
    public void PopulateFleet()
    {
        FleetLog();
        int totalShips = DialogueLua.GetVariable("Ships").asInt;

        ClearChildren(Slots.transform);

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

    public void DisplaySlot(int selectedSlotNumber)
    {
        Image[] slots = Slots.GetComponentsInChildren<Image>(true);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
  
        if(slots[selectedSlotNumber].GetComponent<PrototypeVehicle>())
        {
            //has a vehicle
            DisplayShipName(fleetNameArray[selectedSlotIndex]);
            salvageButton.gameObject.SetActive(true);
            cargoButton.gameObject.SetActive(true);
            flyButton.gameObject.SetActive(true);
            createButton.gameObject.SetActive(false);
            slots[selectedSlotNumber].GetComponentInChildren<Text>().enabled = false;

        }
        else
        {
            //empty
            DisplayShipName("Empty");
            createButton.gameObject.SetActive(true);
            salvageButton.gameObject.SetActive(false);
            cargoButton.gameObject.SetActive(false);
            flyButton.gameObject.SetActive(false);

        }

        slots[selectedSlotNumber].gameObject.SetActive(true);
        slotNumber.text = (selectedSlotIndex + 1).ToString("0/") + availableSlots.ToString("0");

    }

    //NAVIGATION
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

    
    public void DisplayShipName(string shipName)
    {
        currentShip.text = shipName;
        DialogueLua.SetVariable("Ship Name", shipName);

    }
}
