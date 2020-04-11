using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool finished = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Finished()
    {
        finished = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
