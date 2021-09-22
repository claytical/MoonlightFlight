using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tinker : MonoBehaviour
{
    // Start is called before the first frame update
    public VehicleType vehicle;
    public Text amountOfConsciousnessToSpend;

    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void initializeTinker()
    {
        amountOfConsciousnessToSpend.text = PlayerPrefs.GetInt("consciousness", 0).ToString("0");
        TinkerButton[] buttons = GetComponentsInChildren<TinkerButton>();
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].intializeStats();
        }

    }

    public void UseConsciousness() { 
        PlayerPrefs.SetInt("consciousness", System.Int32.Parse(amountOfConsciousnessToSpend.text));
    }
}
