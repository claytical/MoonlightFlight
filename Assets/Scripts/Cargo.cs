using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cargo : MonoBehaviour
{
    public PowerUp.Reward powerUp;
    public UsePowerUps powerUpControl;
    public Image icon;

    public void Use()
    {
        powerUpControl.powerUpInUse = powerUp;
        powerUpControl.cargoInUse.sprite = icon.sprite;
    }
}
