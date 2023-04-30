using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;


[System.Serializable]
public class CargoHold
{
    public int amount;
    public Cargo cargo;
}



public class UsePowerUps : MonoBehaviour
{
    public ProceduralLevel level;
    public Playground playground;
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

            int storedCargo = PlayerPrefs.GetInt(DialogueLua.GetVariable("Ship Name").asString + cargo[i].cargo.powerUp.ToString(), 0);
            if(cargo[i].cargo.powerUp == PowerUp.Reward.Nuke)
            {
                DialogueLua.SetVariable("Nukes", storedCargo);
            }

            if (cargo[i].cargo.powerUp == PowerUp.Reward.Stop)
            {
                DialogueLua.SetVariable("Brakes", storedCargo);

            }

            cargo[i].amount = storedCargo;
            //cargo transfered.
            PlayerPrefs.SetInt(DialogueLua.GetVariable("Ship Name").asString + cargo[i].cargo.powerUp.ToString(), 0);


            if (cargo[i].amount > 0)
            {
                //setting default, last one available on list will be used.
                if(cargo[i].cargo.GetComponent<Button>())
                {
                    cargo[i].cargo.GetComponent<Button>().interactable = true;

                }
                if (cargo[i].cargo.GetComponentInChildren<Text>())
                {

                    cargo[i].cargo.GetComponentInChildren<Text>().text = cargo[i].amount.ToString("0");
                }
                                
            }
            else
            {
                if (cargo[i].cargo.GetComponent<Button>())
                {
                    cargo[i].cargo.GetComponent<Button>().interactable = false;
                }

            }
        }

    }


    public void AddPowerUp(PowerUp.Reward reward, int amount)
    {
        Debug.Log("Adding " + reward.ToString());
  //this variable could have some issues with being separated from dialoguelua
        //In game, cargo capacity goes beyond maximum. rewriting for lua
        for(int i = 0; i < cargo.Length; i++)
        {
            if(cargo[i].cargo.powerUp == reward)
            {
                cargo[i].amount += amount;
                Debug.Log("Incremented " + reward.ToString());

                //                int currentAmount = PlayerPrefs.GetInt(lot.vehicle.name + reward.ToString(), 0);
                //                currentAmount += amount;
                //                cargo[i].amount = currentAmount;
                //                PlayerPrefs.SetInt(lot.vehicle.name + reward.ToString(), cargo[i].amount);
                cargo[i].cargo.GetComponentInChildren<Text>().text = cargo[i].amount.ToString("0");
                cargo[i].cargo.GetComponent<Button>().interactable = true;
                break;
            }
        }
    }

    public void TransferCargo()
    {
        for(int i = 0; i < cargo.Length; i++)
        {
            cargo[i].amount = 0;
            cargo[i].cargo.GetComponentInChildren<Text>().text = 0.ToString("0");

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
                    cargo[i].amount--;
                    cargo[i].cargo.GetComponentInChildren<Text>().text = cargo[i].amount.ToString("0");
                    if(cargo[i].amount <= 0)
                    {
                        if (cargo[i].cargo.GetComponent<Button>())
                        {
                            cargo[i].cargo.GetComponent<Button>().interactable = false;
                        }
                    }
                }
            }
        }
    }

            /*
                public void Use()

                /*
                for (int i = 0; i < cargo.Length; i++)
                {
                    if (powerUpInUse == cargo[i].cargo.powerUp)
                    {
                        if (cargo[i].amount > 0)
                        {
                            switch (powerUpInUse)
                            {
                                case PowerUp.Reward.Nuke:
                                    DialogueLua.SetVariable("Nukes", DialogueLua.GetVariable("Nukes").asInt - 1);
                                    level.DropNuke();
                                    break;
                                case PowerUp.Reward.Stop:
                                    DialogueLua.SetVariable("Brakes", DialogueLua.GetVariable("Brakes").asInt - 1);
                                    lot.GetVehicle().ApplyHyperBreak();
                                    break;
                            }

                            cargo[i].amount--;

        //                    PlayerPrefs.SetInt(lot.vehicle.name + cargo[i].cargo.powerUp.ToString(), cargo[i].amount);

                            cargo[i].cargo.GetComponentInChildren<Text>().text = cargo[i].amount.ToString("0");

                        }

                        if (cargo[i].amount <= 0)
                        {
                            if (cargo[i].cargo.GetComponent<Button>())
                            {
                                cargo[i].cargo.GetComponent<Button>().interactable = false;
                            }
                        }
                    }
                }
            }
                */
        }
