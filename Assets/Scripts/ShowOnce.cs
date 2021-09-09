using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int galaxies = PlayerPrefs.GetInt("galaxies", 0);
        if(galaxies > 0)
        {
            Debug.Log(galaxies + " galaxies...");
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
