using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour
{

    public Text displayNumber;
    public int currentAmount = 0;
    private int cost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCost(int c)
    {
        cost = c;
    }
    public void IncreaseAmount()
    {
        if(0 >= cost)
        {
            currentAmount++;
            SetAmount();
        }

    }

    public void DecreaseAmount()
    {
        currentAmount--;

        if(currentAmount < 0)
        {
            currentAmount = 0;
        }

        SetAmount();
    }

    private void SetAmount()
    {
        displayNumber.text = currentAmount.ToString("0");
    }
}
