using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerStats : MonoBehaviour
{

    public Image vehicleIcon;
    public HP hp;
    public TextMeshProUGUI collected;
    public GameObject inactivePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetStats(Vehicle vehicle)
    {
        collected.text = 0.ToString();
        hp.SetHPUI(vehicle.currentHP, vehicle.maxHP);


    }
    public void Deactivate()
    {
        inactivePanel.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnergyCollected(int amount)
    {
        collected.text = amount.ToString("0");
    }
}
