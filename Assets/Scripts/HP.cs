using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Transform gauge;
    public GameObject hpUnit;

    public GameObject emptyUnit;
    public Transform hpLeft;

    // Start is called before the first frame update


    public void SetHP(int amount, int maxAmount)
    {
        Debug.Log("Setting HP to " + amount);

        for (int i = 0; i < maxAmount; i++)
        {
            Instantiate(emptyUnit, gauge);
        }

        for (int i = 0; i < amount; i++)
        {
            Instantiate(hpUnit, hpLeft);
        }

    }

    public bool TakeDamage(int amount)
    {
        if (hpLeft.GetComponentsInChildren<Image>().Length >= amount)
        {

            //2; 2 >= 0, 
            for (int i = amount; i > 0; i--)
            {
                Debug.Log("Destroying HP: " + i);
                if(hpLeft.GetComponentsInChildren<Image>().Length > i)
                {
                    GameObject hp = hpLeft.GetComponentsInChildren<Image>()[i].gameObject;
                    Destroy(hp);
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return true;

        }
    }
    public void IncreaseHP(int amount)
    {
        //20 > 15
        if (emptyUnit.GetComponentsInChildren<Image>().Length > hpUnit.GetComponentsInChildren<Image>().Length)
        {
            int availableHP = emptyUnit.GetComponentsInChildren<Image>().Length - hpUnit.GetComponentsInChildren<Image>().Length;
            
            if (availableHP >= amount)
            {
                for (int i = 0; i < amount; i++)
                {
                    Instantiate(hpUnit, hpLeft);
                }
            }
            else
            {
                for(int i = 0; i < availableHP; i++)
                {
                    Instantiate(hpUnit, hpLeft);
                }
            }
        }


    }
}
