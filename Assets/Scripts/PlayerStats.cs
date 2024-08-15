using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public Image vehicleIcon;
    public HP hp;
    public TextMeshProUGUI collected;
    public GameObject inactivePanel;

    private int vehicleID;  // Store the vehicle ID to recreate the vehicle later
    private Vehicle currentVehicle;

    // Start is called before the first frame update
    void Start()
    {
        // Initialization logic can go here if needed
    }

    public void SetStats(Vehicle vehicle)
    {
        // Store the vehicle ID and reference to the vehicle instance
        vehicleID = vehicle.GetInstanceID();
        currentVehicle = vehicle;

        vehicleIcon.sprite = vehicle.GetComponent<SpriteRenderer>().sprite;
        UpdateEnergyDisplay(vehicle.energyCollected);
        Debug.Log("Energy collected should be displayed.");
        hp.SetHPUI(vehicle.currentHP, vehicle.maxHP);
    }

    public void SetColor(Color color)
    {
        vehicleIcon.color = color;
    }

    public bool TakeDamage(int damage)
    {
        return hp.TakeDamage(damage);
    }

    public void Deactivate()
    {
        inactivePanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Continuously update the energy display if the vehicle is active
        if (currentVehicle != null)
        {
            UpdateEnergyDisplay(currentVehicle.energyCollected);
        }
    }

    public void EnergyCollected(int amount)
    {
        collected.text = amount.ToString("0");
    }

    private void UpdateEnergyDisplay(float energy)
    {
        collected.text = Mathf.CeilToInt(energy).ToString();
    }

    // Optional: Method to recreate or reset the vehicle using the stored ID
    public Vehicle RecreateVehicle()
    {
        // Logic to recreate the vehicle based on the stored vehicle ID
        // This will depend on your game's architecture and vehicle management system
        // Example (assuming you have a vehicle manager):
        // return VehicleManager.Instance.GetVehicleByID(vehicleID);

        return null; // Placeholder return, replace with actual logic
    }
}
