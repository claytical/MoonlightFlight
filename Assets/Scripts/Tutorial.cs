using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;



public class Tutorial : MonoBehaviour
{
    public GameObject firstObject;
    // Start is called before the first frame update
    
    void Start()
    {
        if(!DialogueLua.GetVariable("Tutorial").asBool)
        {
            Debug.Log("Running tutorial...");

            firstObject.SetActive(true);
        }
        else
        {
            Debug.Log("Going to the outpost...");
            DialogueManager.PlaySequence("LoadLevel(Outpost)");
        }
    }

    void Update()
    {
    }
}
