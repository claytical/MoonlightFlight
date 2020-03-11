using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOffCollider()
    {
        GetComponent<CircleCollider2D>().enabled = false;

    }
    public void TurnOnCollider()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
}

