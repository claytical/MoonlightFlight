using System.Collections;
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

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll.gameObject.name + " collding with " + this.name);
        if (coll.gameObject.tag == "Bumpable")
        {
            Debug.Log("Hazard Colliding with Bumpable, let's destroy it.");
            Destroy(coll.gameObject);
        }
        if(coll.gameObject.tag == "Disappearing")
        {

            if(gameObject.tag == "Avoid")
            {
                GetComponent<Animator>().SetTrigger("destroy");
            }
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

