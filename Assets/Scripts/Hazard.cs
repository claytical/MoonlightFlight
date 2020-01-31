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
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            //update this logic to allow seeds collected
            coll.gameObject.GetComponentInParent<BallHolder>().player.GameOver("You hit a hazard!", 0);

        }

    }
}
