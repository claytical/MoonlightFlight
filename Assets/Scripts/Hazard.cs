using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public bool randomGravity;
    public int timeUntilDestroy;
    private float timeCreated;
    private Color color;

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


    }

    void ResetColor()
    {
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(GetComponent<SpriteRenderer>())
        {
            color = GetComponent<SpriteRenderer>().color;
            Color newColor = color;
            newColor.a = color.a * .5f;
            GetComponent<SpriteRenderer>().color = newColor;
            Invoke("ResetColor", .1f);
        }

        if (coll.gameObject.tag == "Bumpable")
        {
            if(transform.parent.GetComponentInChildren<Moving>())
            {
                GetComponent<Rigidbody2D>().velocity = transform.parent.GetComponentInChildren<Moving>().GetCurrentDirection();
            }
//            GetComponent<Rigidbody2D>().velocity = coll.rigidbody.velocity;
/*
            if(coll.gameObject.GetComponent<Explode>())
            {
                coll.gameObject.GetComponent<Explode>().Go();
            }
*/
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

