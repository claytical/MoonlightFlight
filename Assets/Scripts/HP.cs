using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public GameObject hpUnit;
    public Transform hpLeft;
    private int amount = 0;
    private int maxAmount = 10;

    // Set up the HP UI with the specified amount
    public void SetHPUI(int _amount)
    {
        amount = Mathf.Clamp(_amount, 0, maxAmount);

        // Clear existing HP units
        foreach (Transform child in hpLeft)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new HP units
        for (int i = 0; i < amount; i++)
        {
            Instantiate(hpUnit, hpLeft);
        }
    }

    // Update the HP UI to reflect the current amount of HP
    public void UpdateHPUI(int _amount)
    {
        amount = Mathf.Clamp(_amount, 0, maxAmount);
        Debug.Log("New Amount: " + amount);

        // Decrease HP (remove units)
        if (amount < hpLeft.childCount)
        {
            int unitsToRemove = hpLeft.childCount - amount;
            for (int i = 0; i < unitsToRemove; i++)
            {
                Destroy(hpLeft.GetChild(hpLeft.childCount - 1).gameObject);
            }
        }

        // Increase HP (add units)
        if (amount > hpLeft.childCount)
        {
            int unitsToAdd = amount - hpLeft.childCount;
            for (int i = 0; i < unitsToAdd; i++)
            {
                Instantiate(hpUnit, hpLeft);
            }
        }
    }

    // Apply damage and update the HP UI
    public bool TakeDamage(int damage)
    {
        amount -= damage;
        UpdateHPUI(amount);
        return amount <= 0;
    }

    // Increase HP and update the HP UI
    public void IncreaseHP(int _amount)
    {
        amount = Mathf.Clamp(amount + _amount, 0, maxAmount);
        UpdateHPUI(amount);
    }
}
