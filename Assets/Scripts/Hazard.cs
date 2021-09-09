﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public bool useGravity;
    public int timeUntilDestroy;
    private float timeCreated;

    // Use this for initialization
    void Start()
    {
        timeCreated = Time.time;
        if(useGravity)
        {
            if(GetComponent<Rigidbody2D>())
            {
                GetComponent<Rigidbody2D>().gravityScale = Random.Range(-1, 1);
//                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time >= timeCreated + timeUntilDestroy)
        {
         //   Destroy(this.gameObject);
        }
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

