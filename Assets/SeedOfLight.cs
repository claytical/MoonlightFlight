using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedOfLight : MonoBehaviour
{
    public Text SeedCounter;
    // Start is called before the first frame update
    void Start()
    {
        int seedsCollected = PlayerPrefs.GetInt("seeds");
        Debug.Log("SEEDS COLLECTED:" + seedsCollected);
        SeedCounter.text = seedsCollected.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
