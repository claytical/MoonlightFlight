using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public GameObject explosion;
    public float respawnTimer = 10f;
    public float lifetime;
    private float explosionTimer;
    private Vector3 originalPosition;

    void Start()
    {
        explosionTimer = Time.time + lifetime;
        originalPosition = transform.position;
    }

    void Update()
    {
        if(lifetime > 0)
        {
            if(Time.time > explosionTimer)
            {
                Permanent();
                lifetime = 0;
            }
        }    
    }
    
    public void BackToPosition()
    {
        transform.position = originalPosition;
    }

    public void Reactivate()
    {
        gameObject.SetActive(true);
    }

    public void UntilNextSet()
    {
//        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Instantiate(explosion, transform.position, Quaternion.identity);
        if(GetComponent<Platform>())
        {
            //TODO: save its inactive state for the next set
        }
        gameObject.SetActive(false);
    }

    public void Temporary(int spawnDelay)
    {
        Debug.Log("Temporary Spawn Called!");
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Instantiate(explosion, transform.position, Quaternion.identity);
        //transform.position = new Vector3(0, 0, 1000);
        gameObject.SetActive(false);
        Invoke("Reactivate", spawnDelay);
    }

    public void Permanent()
    {

        if (GetComponent<Platform>())
        {
            Debug.Log("Permanent Scale");
//            GetComponent<Platform>().Scale();
        }

        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
        if(GetComponentInParent<Remix>())
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Go()
    {
    }

    
}
