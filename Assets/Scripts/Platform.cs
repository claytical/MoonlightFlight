using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool finished = false;
    public bool canBePushed = false;
    public RigidbodyConstraints2D constraints;
    public float gravity;

    // Start is called before the first frame update
    void Start()
    {

        TurnOffCollision();
        //SetConstraints();
        
    }


    void TurnOffCollision()
    {
        if (GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = false;

        }

        if (GetComponent<PolygonCollider2D>())
        {
            GetComponent<PolygonCollider2D>().enabled = false;
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
            if(coll.gameObject.GetComponent<Platform>())
            {
                Debug.Log("Big Hit, Triggering Animation");

                coll.gameObject.GetComponent<Animator>().SetTrigger("hit");

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

    }

}
