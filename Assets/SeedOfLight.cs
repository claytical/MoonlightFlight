using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedOfLight : MonoBehaviour
{
    public Text SeedCounter;
    public string shipName;
    // Start is called before the first frame update
    void Start()
    {
        int seedsCollected = PlayerPrefs.GetInt(shipName + "_seeds");
        SeedCounter.text = seedsCollected.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
