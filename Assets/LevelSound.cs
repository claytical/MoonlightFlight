using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LevelSound : MonoBehaviour {
    public AudioMixerSnapshot maxMode;
    public AudioMixerSnapshot normalMode;
    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void MaxMode()
    {
        maxMode.TransitionTo(0);
    }
    public void NormalMode() { 
        normalMode.TransitionTo(0);
    }
}
