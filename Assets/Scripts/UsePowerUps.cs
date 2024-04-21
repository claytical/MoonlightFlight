using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UsePowerUps : MonoBehaviour
{
    public ProceduralLevel level;
    public ParkingLot lot;
    public PowerUp.Reward powerUpInUse;
    public int powerUpIndex;
    public Image cargoInUse;

    // Start is called before the first frame update
    void Start()
    {

    }


    public void AddPowerUp(PowerUp.Reward reward, int amount)
    {
        Debug.Log("Adding " + reward.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use()
    {
    }

}
