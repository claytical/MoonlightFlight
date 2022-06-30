using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControlPanel : MonoBehaviour
{
    public ShipSlot ShipSlotInUse;
    public GameObject ControlSubPanel;
    public Tinker tinker;
    private List<Button> slots;

    // Start is called before the first frame update
    void Start()
    {
        CheckGarage();
    }
    

    public void CheckGarage()
    {
        FillSlots();
        string[] vehicles = System.Enum.GetNames(typeof(VehicleType));
        if(!PlayerPrefs.HasKey(VehicleType.Boomerang.ToString()))
        {
            PlayerPrefs.SetInt(VehicleType.Boomerang.ToString(), 0);

        }

        for (int i = 0; i < vehicles.Length; i++)
        {
            if (PlayerPrefs.HasKey(vehicles[i]))
            {
                //this ship is available

                if (i <= slots.Count)
                {
                    slots[i].interactable = true;
                }
                else
                {
                    Debug.Log(vehicles[i] + " ship available but no slot available!");
                }
            }
            else
            {
                slots[i].interactable = false;

                Debug.Log(vehicles[i] + " ship not available yet.");

            }
        }
        
        HighlightSelectedShip();

    }
    private void FillSlots()
    {
        slots = new List<Button>();
        for(int i = 0; i < GetComponentsInChildren<ShipSlot>().Length; i++)
        {
            slots.Add(GetComponentsInChildren<ShipSlot>()[i].gameObject.GetComponent<Button>());
        }
    }

    public void TinkerWithVehicle()
    {
        tinker.vehicle = ShipSlotInUse.vehicle;
    }
    public void HighlightSelectedShip()
    {
        GameState gameState = (GameState)FindObjectOfType(typeof(GameState));
        if (gameState != null)
        {
            Debug.Log("No game state available... that's not good.");
        }
        
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].GetComponentInChildren<ShipSlot>().HighlightMarker.enabled = false;
        }

        if(ShipSlotInUse)
        {
            Debug.Log("Using " + ShipSlotInUse.name);
            ShipSlotInUse.HighlightMarker.enabled = true;
            gameState.SetVehicle(ShipSlotInUse.vehicle);
        }
        else if(slots.Count > 0)
        {
            Debug.Log("Using first available ship: " + slots[0].name);
            gameState.SetVehicle(slots[0].gameObject.GetComponent<ShipSlot>().vehicle);
            slots[0].GetComponentInChildren<ShipSlot>().HighlightMarker.enabled = true;
        }
        else
        {
            Debug.Log("No slots available.");
        }
    }

}
