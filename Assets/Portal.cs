using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public int portalCount; //increases the thickness of the lines
    // Start is called before the first frame update
    void Start()
    {
        /*
         *  check player progress
         *  line thickness goes from .1 to 1, number of lines goes from 1 to forever
         * 
         * 
         * 
         * 
         * 
         * * 
         */
        Debug.Log("PORTAL " + portalCount + " CREATED "); 
    }

    public void Set(int portalIndex, int loopCount)
    {
        //portal count goes from 0 to 
        float lineThicknesss = Mathf.Lerp(.1f, 1, portalIndex);
        GetComponent<WavyRainbowLine>().numberOfLines = loopCount;
        Debug.Log("Portal Setup Complete");
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void Complete()
    {
        //GamepadManager.Instance.HidePortal();
    }


}