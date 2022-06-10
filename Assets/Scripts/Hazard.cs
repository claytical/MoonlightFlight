using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public bool randomGravity;
    public int timeUntilDestroy;
    private float timeCreated;

    // Use this for initialization
    void Start()
    {
        timeCreated = Time.time;
        if(randomGravity)
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
        }
    }


    void Update()
    {
        if (GetComponent<OneDirection>())
        {
 //           Debug.Log("Rotating Hazard 90 degrees...");
//            transform.Rotate(90, 0, 90);
        }

    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bumpable")
        {
            Debug.Log("Hazard Colliding with Bumpable, let's destroy it.");
            if(coll.gameObject.GetComponent<Explode>())
            {
                coll.gameObject.GetComponent<Explode>().Go();
            }
        }
        if(coll.gameObject.tag == "Disappearing")
        {

            //SELF DESTRUCTING HAZARDS?
/*
            if(gameObject.tag == "Avoid")
            {
                GetComponent<Animator>().SetTrigger("destroy");
            }
*/
            if (coll.gameObject.GetComponent<Breakable>())
            {

            }
        }
    }

    public void TurnOffCollider()
    {
        GetComponent<PolygonCollider2D>().enabled = false;

    }
    public void TurnOnCollider()
    {
        GetComponent<PolygonCollider2D>().enabled = true;
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}

