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
        Debug.Log("SET COUNT: " + nextSets.Length);

        if (nextSets.Length > 1)
        {
            selectedSetIndex = Random.Range(0, nextSets.Length);
            SetInfo _nextSet = nextSets[selectedSetIndex];
            nextSet = _nextSet;
            return _nextSet;
        }
        else { 

            Debug.Log("List of sets not available, using non arrayed value");
            //default to assigened next set if array of options isn't filled in
            return this.nextSet;            
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (changingMusic)
        {
            Debug.Log("CHANGING MUSIC!");
            if (gameObject.GetComponents<AudioSource>()[0].loop == true)
            {
                gameObject.GetComponents<AudioSource>()[0].loop = false;
            }
            //transition from current procedural set to next one
            if(!gameObject.GetComponents<AudioSource>()[0].isPlaying)
            {
                Debug.Log("Not playing loop anymore.");
                changingMusic = false;
                start.TransitionTo(0f);
                Debug.Log("I'm in " + gameObject.name);
            }
            else
            {
                Debug.Log("Still Looping");
                gameObject.GetComponents<AudioSource>()[0].loop = false;

            }

        }
        */
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
//        nextSet.currentSet.changingMusic = true;
//    changingMusic = true;

    }

}
