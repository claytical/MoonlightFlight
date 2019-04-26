using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {
    float angle = 0;
    float speed = (2 * Mathf.PI) / 2; //2*PI in degress is 360, so you get 5 seconds to complete a circle
    float radius = .008f;
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
            //GetComponentInChildren<Light>().intensity = Random.Range(.2f, 1f);
            /*            transform.Rotate(new Vector3(0, 0, Time.deltaTime * 400.0f));
                        GetComponent<Rigidbody2D>().isKinematic = false;
                        GetComponent<Rigidbody2D>().AddForce(new Vector3(direction, 100f, 0));//Vector3.up * 15f);
              */
            //            Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            //            transform.position = position + Random.insideUnitSphere * .02f;
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
            GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * .7f);          
            if(transform.position.x > 1 || transform.localPosition.x < -1 || transform.localPosition.y > 1 || transform.localPosition.y < -1)
            {
                transform.position = transform.parent.position;
            }            
    }
    }
}
