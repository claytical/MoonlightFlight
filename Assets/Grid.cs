using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public GameObject spawnLocations;
    public GameObject platforms;
    public ProceduralSet currentSet;
    public int numberOfObjectsToPlace;


    // Start is called before the first frame update
    void Start()
    {
        platforms.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup()
    {
        //transition to first part of new audiomixerx
        //turn on platformsx
        //generate new layout based on spawnlocations

    }
}
