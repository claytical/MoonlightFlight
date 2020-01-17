﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProceduralSet : MonoBehaviour
{
    public AudioMixerSnapshot waiting;
    public AudioMixerSnapshot starting;
    public AudioMixerSnapshot finishedGrid;
    public AudioMixerSnapshot bumpedPlatform;
    public AudioMixerSnapshot feverReached;
    public AudioMixerSnapshot feverBroke;
    public AudioMixerSnapshot feverTimeout;
    public AudioMixerSnapshot switched;
    public AudioMixerSnapshot brokeObject;
    

    public Grid nextGrid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Starting()
    {
        starting.TransitionTo(0);
    }

    public void Waiting()
    {
        waiting.TransitionTo(0);
    }

    public void FeverBroke()
    {
        feverBroke.TransitionTo(1);
        Debug.Log("fever broke!");

    }

    public void FeverReached()
    {
        feverReached.TransitionTo(0);
        Debug.Log("fever reached! in prod set");
    }

    public void FinishedGrid()
    {
        //transition from current procedural set to next one
        nextGrid.gameObject.SetActive(true);
        nextGrid.currentSet.Starting();
        //gameObject.SetActive(false);
    }

    public void BrokeObject()
    {
        Debug.Log("Running Broke Object");
        if (brokeObject)
        {
            brokeObject.TransitionTo(0);
            Debug.Log("Transitioned to Broke Object");
        }
        }

    public void BumpedPlatform()
    {
        Debug.Log("Running Bumped Platform");
        if(bumpedPlatform)
        {
            Debug.Log("Transitioned to Bumped Platform");
            bumpedPlatform.TransitionTo(0);

        }
    }

    public void Switched()
    {
        switched.TransitionTo(0);
    }


    public void FeverTimeout()
    {
        Debug.Log("fever timeout!");

        feverTimeout.TransitionTo(0);
    }

}
