using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool finished = false;
    public bool canBePushed = false;
    public bool causesGlitch = false;
    public RigidbodyConstraints2D constraints;
    public float gravity;
    public Vector3 originalScale;
    public bool scaledDown = false;
    public bool scaledUp = false;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        TurnOffCollision();
        //SetConstraints();
        
    }


    public void TurnOffCollision()
    {
        if (GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = false;

        }
        if (GetComponent<CircleCollider2D>())
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }

        if (GetComponent<PolygonCollider2D>())
        {
            GetComponent<PolygonCollider2D>().enabled = false;
        }

    }

    public void TurnOnCollision()
    {
        if (GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = true;

        }

        if(GetComponent<CircleCollider2D>())
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }

        if (GetComponent<PolygonCollider2D>())
        {
            GetComponent<PolygonCollider2D>().enabled = true;
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (GetComponent<OneDirection>())
        {
            GetComponent<Rigidbody2D>().AddForce(GetComponent<OneDirection>().direction);
        }

        if (coll.otherRigidbody.velocity.magnitude > .2f)
        {
            if (coll.gameObject.GetComponent<Platform>())
            {
                Debug.Log("Big Hit, Triggering Animation");
                if(coll.gameObject.GetComponent<Animator>())
                {
                    coll.gameObject.GetComponent<Animator>().SetTrigger("hit");

                }

            }
        }

        if (coll.gameObject.GetComponent<Breakable>())
        {
            GetComponentInParent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<Breakable>().hit);
            //energyCollection.Play();


            if (coll.gameObject.GetComponent<Breakable>().isDead())
            {

                //NO REWARD FOR DEBRIS / HAZARD COLLECTING ENERGY
                //EVENT #1 - BROKE OBJECT
/*
                    GetComponent<Remix>().level.lot.EnergyCollected();
*/
                Destroy(coll.gameObject);

            }
            else
            {
                Debug.Log("Just hitting an object, not destroyed.");

            }


        }
    }
  


    public void SetConstraints()
    {
        GetComponent<Rigidbody2D>().constraints = constraints;
        GetComponent<Rigidbody2D>().gravityScale = gravity;
    }

    public void Disappear() {
            Destroy(this.gameObject);
    }
    public void Finished()
    {
        //CALLED IN INITIAL ANIMATION
        finished = true;
        TurnOnCollision();

    }

}
