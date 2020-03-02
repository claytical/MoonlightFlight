using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMusic : MonoBehaviour
{
    public AudioSource[] loops;
    // Start is called before the first frame update
    void Start()
    {
        loops = gameObject.GetComponentsInChildren<AudioSource>();
    }

    public void LimitLoopsPlaying(LevelGrid currentGrid)
    {
        for (int i = 0; i < loops.Length; i++)
        {
            if(loops[i].isPlaying)
            {
                if (!loops[i].clip.name.Contains(currentGrid.fullEnergy.clip.name))
                {
                    Debug.Log("Found extra clip playing : " + loops[i].clip.name);
                    loops[i].Stop();
                }

            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
