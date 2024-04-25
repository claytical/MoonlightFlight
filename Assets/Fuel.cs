using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fuel : MonoBehaviour
{

    public GameObject fuel;
    public Transform tank;
    public int amount;
    public int capacity = 0;

    // Start is called before the first frame update
    public void FillTank(int _amount)
    {
        capacity = _amount;
        amount = capacity;

        for (int i = 0; i < capacity; i++)
        {
            GameObject go = Instantiate(fuel, tank);
            Image image = go.GetComponent<Image>();
            //image.color = SetAlpha(image, amount, i);
        }
    }

    public void UpdateUI()
    {
        Debug.Log("FUEL UNITS: " + tank.GetComponentsInChildren<Image>().Length);
        for(int i = 0; i < tank.GetComponentsInChildren<Image>().Length; i++)
        {
            Image img = tank.GetComponentsInChildren<Image>()[i];
            if (i >= amount)
            {
                Debug.Log("UNIT " + i + " SET DARK");

                img.color = SetAlpha(img, .1f);
            }
            else
            {
                Debug.Log("UNIT " + i + " SET BRIGHT");

                img.color = SetAlpha(img, 1f);
            }
        }

    }


    private Color SetAlpha(Image img, float a)
    {
        Color color = img.color;
        color.a = a;
        return color;
    }
    public void Boost()
    {
        amount--;

        if (amount < 0)
        {
            amount = 0;
        }
    }

    

    public bool Drain()
    {
        amount--;
        if (amount <= 0)
        {
            amount = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Recharge()
    {
        amount++;
        if(amount >= capacity)
        {
            amount = capacity;
        }

    }
}
