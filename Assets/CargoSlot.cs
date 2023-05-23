using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class CargoSlot : MonoBehaviour
{
    public bool inUse = false;

    [SerializeField]
    public PowerUp.Reward cargo;
    public Image image;
    private Sprite empty;

    // Start is called before the first frame update
    void Start()
    {
        empty = image.sprite;
        if(cargo == PowerUp.Reward.Shield)
        {
            //empty
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop()
    {
        int cargoInStorage = 0;
        switch(cargo)
        {
            case PowerUp.Reward.Nuke:
                cargoInStorage = PlayerPrefs.GetInt(PowerUp.Reward.Nuke + "_Storage", 0);

                break;
            case PowerUp.Reward.Stop:
                cargoInStorage = PlayerPrefs.GetInt(PowerUp.Reward.Nuke + "_Storage", 0);

                break;
        }
        if(cargoInStorage < 1)
        {
            cargoInStorage--;
        }

        if(cargoInStorage <= 0)
        {
            GetComponent<Image>().sprite = empty;
            inUse = false;
        }
        PlayerPrefs.SetInt(cargo + "_Storage", cargoInStorage);
    }

    public void Empty()
    {
        if(inUse)
        {

            inUse = false;
            int cargoInStorage = PlayerPrefs.GetInt(cargo + "_Storage", 0);
            cargoInStorage++;
            PlayerPrefs.SetInt(cargo + "_Storage", cargoInStorage);
            image.sprite = empty;
            int numberOfSpecifiedCargoOnShip = PlayerPrefs.GetInt(DialogueLua.GetVariable("Ship Name").asString + cargo.ToString(), 0);
            numberOfSpecifiedCargoOnShip--;
            if(numberOfSpecifiedCargoOnShip >= 0)
            {
                PlayerPrefs.SetInt(DialogueLua.GetVariable("Ship Name").asString + cargo.ToString(), numberOfSpecifiedCargoOnShip);
            }
            else
            {
                //zeroed out just in case
                Debug.Log("This shouldn't happen...");
                PlayerPrefs.SetInt(DialogueLua.GetVariable("Ship Name").asString + cargo.ToString(), 0);
            }

            GetComponentInParent<ManageCargo>().UpdateSupplyCount();
        }

    }

    public void Fill(CargoSupply cargoSupply)
    {
        int cargoInStorage = PlayerPrefs.GetInt(cargoSupply.cargo + "_Storage", 0);
        if (!inUse)
        {
            if(GetComponentInParent<ManageCargo>().selectedCargo != PowerUp.Reward.Shield)
            {
                if(cargoInStorage > 0)
                {
                    Debug.Log("HAS CARGO AVAILABLE...");
                    cargo = GetComponentInParent<ManageCargo>().selectedCargo;
                    image.sprite = cargoSupply.image;
                    cargoInStorage--;
                    inUse = true;
                }
                else
                {
                    Debug.Log("NO CARGO AVAILABLE TO TRANSFER...");
                }
                PlayerPrefs.SetInt(cargoSupply.cargo + "_Storage", cargoInStorage);
            }
        }
        UpdateCargoCounts(cargoSupply);       

    }

    public void UpdateCargoCounts(CargoSupply cargoSupply)
    {
        if (GetComponentInParent<ManageCargo>())
        {
            int numberOfSpecifiedCargoOnShip = PlayerPrefs.GetInt(DialogueLua.GetVariable("Ship Name").asString + GetComponentInParent<ManageCargo>().selectedCargo.ToString(), 0);
            numberOfSpecifiedCargoOnShip++;
            PlayerPrefs.SetInt(DialogueLua.GetVariable("Ship Name").asString + cargoSupply.cargo.ToString(), numberOfSpecifiedCargoOnShip);
            cargoSupply.SetCount();

        }

    }
}
