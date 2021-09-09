using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{

    public float timeUntilExplosion;
    private bool exploding = false;
    public GameObject touchPoint;
    // Start is called before the first frame update
    void Start()
    {
        timeUntilExplosion = timeUntilExplosion + Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(timeUntilExplosion <= Time.time && !exploding)
        {
            GetComponent<ParticleSystem>().Play();
            exploding = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        if(exploding)
        {
            if(!GetComponent<ParticleSystem>().isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
        */
        float dist = Vector2.Distance(transform.position, touchPoint.transform.position);

        Debug.Log("MY DISTANCE: " + dist);
        if(dist <= 1)
        {
            Destroy(this.gameObject);

        }
    }
        

    public void SetTouchPoint(GameObject go)
    {
        touchPoint = go;
    }
}
