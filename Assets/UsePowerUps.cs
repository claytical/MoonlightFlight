using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class CargoHold
{
    public int amount;
    public Cargo cargo;
}



public class UsePowerUps : MonoBehaviour
{
    public ProceduralLevel level;
    public ParkingLot lot;
    public CargoHold[] cargo;
    public PowerUp.Reward powerUpInUse;
    public int powerUpIndex;
    public Image cargoInUse;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < cargo.Length; i++)
        {
            //TODO: set amount through playerprefs

            if(cargo[i].amount > 0)
            {
                cargo[i].cargo.gameObject.SetActive(true);
                //setting default, last one available on list will be used.
                cargo[i].cargo.Use();
            }
            else
            {
                cargo[i].cargo.gameObject.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use()
    {

        for (int i = 0; i < cargo.Length; i++)
        {
            if (powerUpInUse == cargo[i].cargo.powerUp)
            {
                if (cargo[i].amount > 0)
                {
                    switch (powerUpInUse)
                    {
                        case PowerUp.Reward.Nuke:
                            level.DropNuke();
                            break;
                        case PowerUp.Reward.Stop:
                            lot.GetVehicle().ApplyHyperBreak();
                            break;
                    }

                    cargo[i].amount--;
                }

                if (cargo[i].amount <= 0)
                {
                    cargo[i].cargo.gameObject.SetActive(false);
                }
            }
        }
    }
}
