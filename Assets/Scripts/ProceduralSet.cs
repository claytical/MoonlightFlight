using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProceduralSet : MonoBehaviour
{
    public AudioMixerSnapshot waiting;
    public AudioMixerSnapshot starting;
    public AudioMixerSnapshot finishedGrid;
    public AudioMixerSnapshot maxEnergy;


    private int selectedGridIndex;


    public void Starting()
    {
        starting.TransitionTo(0);
    }

    public void Waiting()
    {
        waiting.TransitionTo(.1f);
    }

    public void LowEnergy()
    {
        starting.TransitionTo(0);
        Debug.Log("energy track ended");

    }

    public void MaxEnergy()
    {

        maxEnergy.TransitionTo(0);
    }

}
