using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public GameObject hpUnit;
    public Transform hpLeft;
    private int amount = 0;
    private int maxAmount = 0;

    // Start is called before the first frame update
        public void SetHPUI(int _amount, int _maxAmount)
        {
        amount = _amount;
        maxAmount = _maxAmount;
        
            for (int i = 0; i < maxAmount; i++)
            {
                GameObject go = Instantiate(hpUnit, hpLeft);
                Image image = go.GetComponent<Image>();
                image.color = SetAlpha(image, amount, i);
            }
    }

    public void UpdateHPUI(int amount)
    {
        for(int i = 0; i < hpLeft.GetComponentsInChildren<Image>().Length; i++)
        {
            hpLeft.GetComponentsInChildren<Image>()[i].color = SetAlpha(hpLeft.GetComponentsInChildren<Image>()[i], amount, i);
        }
    }

    private Color SetAlpha(Image img, int amount, int index)
    {
        Color color = img.color;
        if (index < amount)
        {
            color.a = 1;
        }
        else
        {
            color.a = .1f;
        }
        return color;
    }

    public bool TakeDamage(int damage)
    {
        amount -= damage;
        UpdateHPUI(amount);
        if (amount <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void IncreaseHP(int _amount)
    {
        amount += _amount;
        UpdateHPUI(amount);
        if(amount >= maxAmount)
        {
            amount = maxAmount;
        }
    }
}
