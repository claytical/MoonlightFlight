using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class SolarFlare : MonoBehaviour
{
    public ParticleSystem.MinMaxGradient[] minMaxGradient;
    public bool setScaleOnStart = true;
    // Start is called before the first frame update
    void Start()
    {
        if (setScaleOnStart)
        {
            float scale = DialogueLua.GetVariable("Energy Available").asFloat * 1.5f;
            if (scale == 0)
            {
                Debug.Log("Scale not found, defaulting to random size");
                scale = Random.Range(40, 400);
            }
            //            transform.localScale = new Vector3(scale, scale, scale);
        }
        if (GetComponentInChildren<Core>())
        {
            GetComponentInChildren<Core>().SetScale();
        }
    }

    public void SetCoreScale()
    {
        float scale = DialogueLua.GetVariable("Energy Available").asFloat * 10f;
        //        transform.localScale = new Vector3(scale, scale, scale);
        if (GetComponentInChildren<Core>())
        {
            GetComponentInChildren<Core>().SetScale();
        }

    }



}
