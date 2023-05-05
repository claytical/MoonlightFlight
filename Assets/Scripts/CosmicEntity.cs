using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class CosmicEntity : MonoBehaviour
{
/*    public ShipSlot ShipSlotInUse;
    public GameObject ControlSubPanel;
    public Tinker tinker;
    private List<Button> slots;
*/
    // Start is called before the first frame update
    void Start()
    {
//        CheckGarage();
    }


    public void SaveCargo()
    {
        PlayerPrefs.SetInt(PowerUp.Reward.Nuke.ToString() + "_Storage", DialogueLua.GetVariable("Nukes").asInt);
        PlayerPrefs.SetInt(PowerUp.Reward.Stop.ToString() + "_Storage", DialogueLua.GetVariable("Brakes").asInt);

        DialogueLua.SetVariable("Nukes", 0);
        DialogueLua.SetVariable("Brakes", 0);
    }

    public void CreateShip() {
        int totalShips = PlayerPrefs.GetInt("Ships", 0);
        List<int> fleetTypes = PlayerPrefsX.GetIntArray("Fleet Ship Types").ToList();
        List<string> fleetNames = PlayerPrefsX.GetStringArray("Fleet Ship Names").ToList();
        totalShips++; 

        DialogueLua.SetVariable("Ships", totalShips);
        PlayerPrefs.SetInt("Ships", totalShips);

        fleetTypes.Add(DialogueLua.GetVariable("Vehicle Type").asInt);
        fleetNames.Add(DialogueLua.GetVariable("Ship Name").asString);
        PlayerPrefsX.SetIntArray("Fleet Ship Types", fleetTypes.ToArray());
        PlayerPrefsX.SetStringArray("Fleet Ship Names", fleetNames.ToArray());
        PlayerPrefs.SetInt(DialogueLua.GetVariable("Ship Name").asString + PowerUp.Reward.Nuke.ToString(), 0);
        PlayerPrefs.SetInt(DialogueLua.GetVariable("Ship Name").asString + PowerUp.Reward.Stop.ToString(), 0);
        PlayerPrefsX.SetIntArray("Fleet Ship Types", fleetTypes.ToArray());
        PlayerPrefsX.SetStringArray("Fleet Ship Names", fleetNames.ToArray());
        if(totalShips - 1 < 0)
        {
            Debug.Log("Ran Create Ship with ID " + fleetTypes[totalShips - 1] + " named " + fleetNames[totalShips - 1]);
        }
        else
        {
            Debug.Log("total ships is zero...");
        }

        string s = PersistentDataManager.GetSaveData(); // Save state.

    }

}
