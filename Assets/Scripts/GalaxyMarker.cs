using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyMarker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, Camera.main.transform.position);

        float solidDistance = 5f; // distance to be 100% solid
       
        if (dist < solidDistance) dist = solidDistance;
           for(int i = 0; i < GetComponentsInChildren<TextMesh>().Length; i++)
            {
                Color c = GetComponentsInChildren<TextMesh>()[i].color;
                c.a = solidDistance / dist;            
                GetComponentsInChildren<TextMesh>()[i].color = c;
            }

    }
}
