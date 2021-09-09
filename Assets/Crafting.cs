using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    public Text partsAvailable;
    public GameObject vehicleToCraft;
    public CraftableVehicle[] allVehicles;
    public List<CraftableVehicle> vehiclesAvailable;
    private int vehicleIndex = 0;
    public Button craftButton;
    public GameObject SelectShipSlot;

    // Start is called before the first frame update
    void Start()
    {
        PopulateAvailableVehicles();
        ShowCraftableVehicle();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextCraftableVehicle()
    {
        vehicleIndex++;
        if(vehicleIndex == vehiclesAvailable.Count)
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
        vehicleToCraft = Instantiate(vehiclesAvailable[vehicleIndex].gameObject, SelectShipSlot.transform);
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
        int parts = PlayerPrefs.GetInt("parts", 0);
        parts = parts - vehicleToCraft.GetComponent<CraftableVehicle>().partsRequired;
        PlayerPrefs.SetInt("parts", parts);
        PlayerPrefs.SetInt(vehicleToCraft.GetComponent<CraftableVehicle>().vehicle.ToString(), 0);
    }
    public void PopulateAvailableVehicles()
    {
        partsAvailable.text = PlayerPrefs.GetInt("parts", 0).ToString();
        vehiclesAvailable.Clear();
        for (int i = 0; i < allVehicles.Length; i++)
        {
            if (!PlayerPrefs.HasKey(allVehicles[i].vehicle.ToString()))
            {
                vehiclesAvailable.Add(allVehicles[i]);
            }
        }
    }
}
