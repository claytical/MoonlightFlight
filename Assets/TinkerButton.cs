using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TinkerButton : MonoBehaviour
{
    public Text statAmount;
    public Tinker tinker;
    public string statToAdjust;
    private int startingTinkerAmount;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void intializeStats()
    {
        startingTinkerAmount = PlayerPrefs.GetInt(tinker.vehicle.ToString("") + "_" + statToAdjust, 0);
        statAmount.text = startingTinkerAmount.ToString("0");
        Debug.Log("Starting Tinker Amount is: " + startingTinkerAmount);

    }

    private bool isThereConsciousnessLeft()
    {
        int c = System.Int32.Parse(tinker.amountOfConsciousnessToSpend.text);
        if (c > 0)
        {
            return true;
        }
        return false;
    }
    public void TempAddConsciousness()
    {
        if (isThereConsciousnessLeft())
        {
            int c = System.Int32.Parse(statAmount.text);
            c++;
            statAmount.text = c.ToString("0");
            int cLeft = System.Int32.Parse(tinker.amountOfConsciousnessToSpend.text);
            cLeft--;
            tinker.amountOfConsciousnessToSpend.text = cLeft.ToString("0");
        }
    }

    public void TempReduceConsciousness()
    {
        int c = System.Int32.Parse(statAmount.text);

        if (startingTinkerAmount >= c)
        {
            //use consciousness
            if (isThereConsciousnessLeft())
            {
                if (c >= 1)
                {
                    int cLeft = System.Int32.Parse(tinker.amountOfConsciousnessToSpend.text);
                    cLeft--;
                    tinker.amountOfConsciousnessToSpend.text = cLeft.ToString("0");

                    c--;
                }

            }

        }
        else
        {
            //user decided to not add consciousness they temporarily added
            if (c >= 1)
            {
                int cLeft = System.Int32.Parse(tinker.amountOfConsciousnessToSpend.text);
                cLeft++;
                tinker.amountOfConsciousnessToSpend.text = cLeft.ToString("0");
                c--;
            }

        }
        statAmount.text = c.ToString("0");
    }

    public void SaveChanges()
    {
        int currentStat = PlayerPrefs.GetInt(tinker.vehicle.ToString() + "_" + statToAdjust, 0);
        PlayerPrefs.SetInt(tinker.vehicle.ToString() + "_" + statToAdjust, currentStat + System.Int32.Parse(statAmount.text));
        
    }
}
