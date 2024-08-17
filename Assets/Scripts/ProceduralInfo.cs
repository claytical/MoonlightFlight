using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class ProceduralInfo : MonoBehaviour
{
    public AudioMixerSnapshot start;
    public AudioMixerSnapshot finish;
    public SetInfo[] nextSets; // Array of possible next sets

    private int selectedSetIndex;
    public bool changingMusic = true;

    public SetInfo SetNextSet()
    {
        if (nextSets.Length > 1)
        {
            selectedSetIndex = PickWeightedSet();
            SetInfo nextSet = nextSets[selectedSetIndex];
            return nextSet;
        }
        else
        {
            Debug.Log("List of sets not available, using first of array");
            if (nextSets.Length > 0)
            {
                return nextSets[0];
            }
            else
            {
                Debug.LogWarning("No next sets available!");
                return null;
            }
        }
    }

    private int PickWeightedSet()
    {
        int totalWeight = 0;
        foreach (SetInfo set in nextSets)
        {
            totalWeight += set.weight;
        }

        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (SetInfo set in nextSets)
        {
            cumulativeWeight += set.weight;
            if (randomValue < cumulativeWeight)
            {
                return Array.IndexOf(nextSets, set);
            }
        }

        return 0; // Fallback to the first set if something goes wrong
    }

    public void Starting()
    {
        start.TransitionTo(0.2f);
    }

    public void Waiting()
    {
        start.TransitionTo(0.1f);
    }

    public void LowEnergy()
    {
        start.TransitionTo(0);
    }

    public void MaxEnergy()
    {
        finish.TransitionTo(0);
    }

    public void FinishedSet()
    {
        // This method is called when the current set finishes
        Debug.Log("Set finished");
    }
}
