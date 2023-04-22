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

    public void Fill()
    {
//        int cargoInStorage = PlayerPrefs.GetInt(DialogueLua.GetVariable("Ship Name").asString + GetComponentInParent<ManageCargo>().selectedCargo.ToString(), 0);
        int cargoInStorage = PlayerPrefs.GetInt(GetComponentInParent<ManageCargo>().selectedCargo + "_Storage", 0);

        bool updateCargo = false;

        if (!inUse)
        {
            if(GetComponentInParent<ManageCargo>().selectedCargo != PowerUp.Reward.Shield)
            {
                if(cargoInStorage > 0)
                {
                    Debug.Log("HAS CARGO AVAILABLE...");
                    cargo = GetComponentInParent<ManageCargo>().selectedCargo;
                    image.sprite = GetComponentInParent<ManageCargo>().currentSelectedButton.gameObject.GetComponent<CargoSupply>().image;
                    GetComponentInParent<ManageCargo>().currentSelectedButton.Select();
                    cargoInStorage--;
                    updateCargo = true;
                    inUse = true;
                }
                else
                {
                    Debug.Log("NO CARGO AVAILABLE TO TRANSFER...");
                }
            }
        } else 
        {
            inUse = false;
            if (cargo == PowerUp.Reward.Nuke)
            {
                //put nuke back into cold storage
                cargoInStorage++;
                updateCargo = true;

            }
            if (cargo == PowerUp.Reward.Stop)
            {
                cargoInStorage++;
                //put brake back into cold storage
                updateCargo = true;

            }

            image.sprite = empty;

        }
        if(updateCargo)
        {
            if(GetComponentInParent<ManageCargo>())
            {
                PlayerPrefs.SetInt(GetComponentInParent<ManageCargo>().selectedCargo + "_Storage", cargoInStorage);
                int numberOfSpecifiedCargoOnShip = PlayerPrefs.GetInt(DialogueLua.GetVariable("Ship Name").asString + GetComponentInParent<ManageCargo>().selectedCargo.ToString(), 0);
                numberOfSpecifiedCargoOnShip++;
                PlayerPrefs.SetInt(DialogueLua.GetVariable("Ship Name").asString + GetComponentInParent<ManageCargo>().selectedCargo.ToString(), numberOfSpecifiedCargoOnShip);
                GetComponentInParent<ManageCargo>().currentSelectedButton.GetComponent<CargoSupply>().count.text = cargoInStorage.ToString("0");

            }

        }

    }
}
