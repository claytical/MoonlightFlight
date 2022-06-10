using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    // Use this for initialization

    void Update()
    {
        if(GetComponent<ParticleSystem>())
        {
            if(!GetComponent<ParticleSystem>().isPlaying)
            {
                Destroy(this.gameObject);
            }
        }    
    }
}
