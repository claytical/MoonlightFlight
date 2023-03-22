using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class LoadGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        PixelCrushers.SaveSystem.LoadFromSlot(1);
        PixelCrushers.SaveSystem.LoadScene("Main");
    }

    // Update is called once per frame

    public void SaveProgress()
    {
        Debug.Log("Saving Progress...");
        PixelCrushers.SaveSystem.SaveToSlot(1);
    }

    void Update()
    {
        
    }
}
