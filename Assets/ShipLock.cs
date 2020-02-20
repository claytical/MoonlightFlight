using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLock : MonoBehaviour
{
    public int seedsRequired;

    // Start is called before the first frame update
    void Start()
    {
        int seedsCollected = PlayerPrefs.GetInt("seeds", 0);
        if(seedsCollected >= seedsRequired)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
