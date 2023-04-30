using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class Cargo : MonoBehaviour
{
    public PowerUp.Reward powerUp;
    public UsePowerUps powerUpControl;
    public Image icon;

    public void Use()
    {
        powerUpControl.powerUpInUse = powerUp;
//        powerUpControl.cargoInUse.sprite = icon.sprite;

        switch (powerUp)
        {
            case PowerUp.Reward.Nuke:
                DialogueLua.SetVariable("Nukes", DialogueLua.GetVariable("Nukes").asInt - 1);
                if(powerUpControl.level)
                {
                    powerUpControl.level.DropNuke();
                }
                if(powerUpControl.playground)
                {
                    powerUpControl.playground.DropNuke();
                }
                break;

            case PowerUp.Reward.Stop:
                DialogueLua.SetVariable("Brakes", DialogueLua.GetVariable("Brakes").asInt - 1);
                powerUpControl.lot.GetVehicle().ApplyHyperBreak();
                break;
        }

        powerUpControl.Use();

    }

}
