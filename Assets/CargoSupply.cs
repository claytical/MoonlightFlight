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
    // Start is called before the first frame update
    void Start()
    {
        count.text = PlayerPrefs.GetInt(cargo.ToString() + "_Storage", 0).ToString("0");
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
