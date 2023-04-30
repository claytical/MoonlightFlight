using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class CargoSupply : MonoBehaviour
{
    public PowerUp.Reward cargo;
    public ManageCargo cargoManager;
    public Sprite image;
    public Text count;
    private int amount;
    // Start is called before the first frame update
    void Start()
    {
        amount = PlayerPrefs.GetInt(cargo.ToString() + "_Storage", 0);
        count.text = amount.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCargo()
    {
        cargoManager.currentSelectedButton = GetComponent<Button>();
        cargoManager.SetCargo(cargo);

    }

}
