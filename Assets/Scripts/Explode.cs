using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public GameObject explosion;
    public float respawnTimer;
    public float lifetime;
    private float explosionTimer;


    void Start()
    {
        explosionTimer = Time.time + lifetime;
        
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
            transform.parent.parent.GetComponent<SetInfo>().objectsToRespawn.Add(respawn);

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
