using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class Storage : MonoBehaviour
{
    public ProceduralLevel level;
    // Start is called before the first frame update
    void Start()
    {
        SendCurrentCargoToStorage();
        level.WarpBack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendCurrentCargoToStorage()
    {
        Debug.Log("Sending Cargo to Cold Storage...");
        int nukesCollected = DialogueLua.GetVariable("Nukes").asInt;
        int brakesCollected = DialogueLua.GetVariable("Brakes").asInt;
        nukesCollected += PlayerPrefs.GetInt(PowerUp.Reward.Nuke.ToString() + "_Storage", 0);
        brakesCollected += PlayerPrefs.GetInt(PowerUp.Reward.Stop.ToString() + "_Storage", 0);
        PlayerPrefs.SetInt(PowerUp.Reward.Nuke.ToString() + "_Storage", nukesCollected);
        PlayerPrefs.SetInt(PowerUp.Reward.Stop.ToString() + "_Storage", brakesCollected);

        level.lot.PowerUps.TransferCargo();
        DialogueLua.SetVariable("Nukes", 0);
        DialogueLua.SetVariable("Brakes", 0);
        this.gameObject.SetActive(false);
    }
}
