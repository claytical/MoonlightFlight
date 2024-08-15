using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    public GameObject fuel;
    public Transform tank;
    private int amount;
    private int capacity;

    // Fill the tank with the specified amount of fuel units
    public void FillTank(int _capacity)
    {
        capacity = _capacity;
        amount = capacity;

        // Clear existing fuel units
        foreach (Transform child in tank)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new fuel units
        for (int i = 0; i < capacity; i++)
        {
            Instantiate(fuel, tank);
        }
    }

    // Update the fuel UI to reflect the current amount of energy left
    public void UpdateFuelUI(int _amount)
    {
        amount = _amount;

        // Clear existing fuel units
        foreach (Transform child in tank)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the remaining fuel units
        for (int i = 0; i < amount; i++)
        {
            Instantiate(fuel, tank);
        }
    }
}
