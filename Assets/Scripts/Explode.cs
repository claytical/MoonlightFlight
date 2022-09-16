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
                Go();
                lifetime = 0;
            }
        }    
    }
    
    public void BackToPosition()
    {
        transform.position = originalPosition;
    }

    public void Temporary()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Instantiate(explosion, transform.position, Quaternion.identity);
        transform.position = new Vector3(0, 0, 1000);
        Invoke("BackToPosition", 1);
    }

    public void Go()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Instantiate(explosion, transform.position, Quaternion.identity);
        if(respawnTimer > 0)
        {
            GameObject go = Instantiate(this.gameObject, transform.parent);
            go.SetActive(false);
            ObjectRespawn respawn = new ObjectRespawn();
            respawn.respawnedObject = go;
            respawn.timeUntilActive = respawnTimer + Time.time;
            Debug.Log("ADDING TO RESPAWN POOL:" + transform.parent.parent.name);
            if(transform.parent.parent.GetComponent<SetInfo>())
            {
                transform.parent.parent.GetComponent<SetInfo>().objectsToRespawn.Add(respawn);

            }
            if (transform.parent.parent.parent.GetComponent<SetInfo>())
            {
                transform.parent.parent.parent.GetComponent<SetInfo>().objectsToRespawn.Add(respawn);

            }
        }

        if (GetComponent<Moving>())
        {
            GetComponent<Moving>().speed = 0;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
