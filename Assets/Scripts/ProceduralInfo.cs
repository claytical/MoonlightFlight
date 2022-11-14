using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProceduralInfo : MonoBehaviour
{

    public AudioMixerSnapshot start;
    public AudioMixerSnapshot finish;
    public SetInfo nextSet;
    public SetInfo[] nextSets;

    private int selectedSetIndex;
    public bool changingMusic = true;

    public SetInfo SetNextSet()
    {

        if (nextSets.Length > 1)
        {
            selectedSetIndex = PickWeightedSet();
            SetInfo _nextSet = nextSets[selectedSetIndex];
            //in case the next set hasn't been set on this object
            nextSet = _nextSet;
            return _nextSet;
        }
        else { 

            Debug.Log("List of sets not available, using first of array");
            //default to assigened next set if array of options isn't filled in
            nextSet = nextSets[0];
            return nextSets[0];            
        }
    }


    public int PickWeightedSet()
    {
        int[] setRange = new int[nextSets.Length];
        int total = 0;
        for (int i = 0; i < nextSets.Length; i++)
        {
            if(nextSets[i].weight <= 0)
            {
                //don't let the value go less than 1
                nextSets[i].weight = 1;
            }
            total += nextSets[i].weight;
            setRange[i] = total;
        }

        int roll = Random.Range(0, total);
        int selectedSet = -1;

        for (int i = 1; i <= setRange.Length; i++)
        {
            if (roll > setRange[i - 1] && roll < setRange[i])
            {
                selectedSet = i;
            }
        }

        if (selectedSet == -1)
        {
            selectedSet = 0;
        }

        //decrease the chance of the next set being chosen
        nextSets[selectedSet].weight--;
        return selectedSet;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Starting()
    {

        start.TransitionTo(.2f);
    }

    public void Waiting()
    {
        start.TransitionTo(.1f);
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
        //PLAY TRANSITION EFFECT
        nextSet.gameObject.SetActive(true);
        Debug.Log("TURNING ON " + nextSet.name);

    }

}
