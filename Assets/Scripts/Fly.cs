using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {
    public bool free = false;
    public int timeFreed;
    public GameObject ball;
    private float direction;
    // Use this for initialization
    void Start () {
        direction = Random.Range(-10f, 10f);
    }

    // Update is called once per frame
    void Update () {
        if (free)
        {
            if (ball)
            {
                transform.position = Vector3.MoveTowards(transform.position, ball.transform.position, Time.deltaTime);
            }

            if (timeFreed < Time.frameCount)
            {
                Destroy(gameObject);
            }
 
        }
        else
        {
            /*
              
            Vector3 extents = transform.parent.GetComponentInParent<Breakable>().sprites[0].GetComponent<SpriteRenderer>().bounds.extents;

            extents = extents / 2;
            extents += transform.parent.position;

            if(transform.position.x > extents.x || transform.position.x < -extents.x || transform.position.y > extents.y || transform.position.y < -extents.y)
            {
                transform.position = transform.parent.position;
            }

            */

            GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * 4f);          
            if(transform.localPosition.x > 1 || transform.localPosition.x < -1 || transform.localPosition.y > 1 || transform.localPosition.y < -1)
            {
                transform.position = transform.parent.position;
            }            
    }
    }
}
