using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public bool randomGravity;
    public float timeUntilDestroy;
    private float timeCreated;
    private Color color;
    public float scaleSpeed = .1f;
    private float scaleDirection = -1;
    private bool scaling = false;
    private Vector3 originalScale;
    // Use this for initialization
    void Start()
    {
        originalScale = transform.localScale;
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
        if(scaling)
        {
            Debug.Log("Scaling: " + scaleDirection);
            Vector3 newScale = new Vector3();

            newScale.x = transform.localScale.x + (scaleSpeed * scaleDirection);
            newScale.y = transform.localScale.y + (scaleSpeed * scaleDirection);
            newScale.z = transform.localScale.z + (scaleSpeed * scaleDirection);
            transform.localScale = newScale;
            Debug.Log("Scaling: " + scaleDirection + " SCALE: " + newScale + "T SCALE: " + transform.localScale);

            if (newScale.x <= 0)
            {
                scaleDirection *= -1;
            }
            if(newScale.x >= originalScale.x)
            {
                scaleDirection *= -1;
                scaling = false;
                transform.localScale = originalScale;
            }
        }

    }

    void ResetColor()
    {
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(GetComponent<Animator>())
        {
            GetComponent<Animator>().enabled = false;

        }
        scaling = true;
        if(GetComponent<SpriteRenderer>())
        {
            /*
            color = GetComponent<SpriteRenderer>().color;
            Color newColor = color;
            newColor.a = color.a * .5f;
            GetComponent<SpriteRenderer>().color = newColor;
            Invoke("ResetColor", .1f);
        */
            }

        if (coll.gameObject.tag == "Bumpable")
        {
            if(transform.parent.GetComponentInChildren<Moving>())
            {
                GetComponent<Rigidbody2D>().velocity = transform.parent.GetComponentInChildren<Moving>().GetCurrentDirection();
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
}

