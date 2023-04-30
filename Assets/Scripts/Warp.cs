using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public ProceduralLevel level;
    public Playground playground;
    

    void Start()
    {
        Time.timeScale = 0;    
    }

    void Update()
    {
    }
    public void Disengage()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void Outpost()
    {
        //activate cosmic
        //move camera away from game space
        if(level) {
            level.Warp();
        }
        if(playground)
        {
            playground.Warp();
        }
        //turn off menu
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
