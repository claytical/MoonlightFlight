using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class Garage : MonoBehaviour
{
    public GameObject fleetPanel;
    public Text parts;
    public Text ships;
    public Text nukes;
    public Text brakes;
    // Start is called before the first frame update
    void Start()
    {
        parts.text = DialogueLua.GetVariable("Parts").asString + " Parts";
        ships.text = DialogueLua.GetVariable("Ships").asString + " Ships";
        nukes.text = PlayerPrefs.GetInt(PowerUp.Reward.Nuke.ToString() + "_Storage", 0).ToString("0 Nukes");
        brakes.text = PlayerPrefs.GetInt(PowerUp.Reward.Stop.ToString() + "_Storage", 0).ToString("0 Brakes");
        /*
        if (DialogueLua.GetVariable("Ships").asInt <= 0)
        {
            //Force Ship Creation
            fleetPanel.SetActive(true);
            gameObject.SetActive(false);

        }
        else
        {
            fleetPanel.SetActive(false);
            gameObject.SetActive(true);

        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
