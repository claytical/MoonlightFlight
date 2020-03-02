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


    public LevelGrid[] nextGrids;

    private int selectedGridIndex;
    public LevelGrid nextGrid;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public LevelGrid SetNextGrid()
    {
        selectedGridIndex = Random.Range(0, nextGrids.Length);
        Debug.Log("Selected Grid " + selectedGridIndex);
        LevelGrid _nextGrid = nextGrids[selectedGridIndex];
        nextGrid = _nextGrid;
        Debug.Log("SELECTED GRID: " + nextGrids[selectedGridIndex].name);

        return _nextGrid;
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

    public void FinishedGrid()
    {
        //transition from current procedural set to next one
        finishedGrid.TransitionTo(0);
        Debug.Log("I'm in " + gameObject.name);
        nextGrid.gameObject.SetActive(true);
        Debug.Log("TURNING ON " + nextGrid.name);
    }
    /*
    public void BrokeObject()
    {
        Debug.Log("Running Broke Object");
        if (brokeObject)
        {
            brokeObject.TransitionTo(0);
            Debug.Log("Transitioned to Broke Object");
        }
        }
        */
    /*
public void BumpedPlatform()
{
    Debug.Log("Running Bumped Platform");
    if(bumpedPlatform)
    {
        Debug.Log("Transitioned to Bumped Platform");
        bumpedPlatform.TransitionTo(0);

    }
}
*/
    /*
        public void Switched()
        {
            switched.TransitionTo(0);
        }
    */
    /*
        public void FeverTimeout()
        {
            Debug.Log("fever timeout!");

            feverTimeout.TransitionTo(.1f);
        }
        */
}
