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
    private Quaternion originalRotation;
    private Vector3 originalScale;

    void Start()
    {
        explosionTimer = Time.time + lifetime;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (lifetime > 0)
        {
            if (Time.time > explosionTimer)
            {
                Permanent();
                lifetime = 0;
            }
        }
    }

    public void BackToPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    public void Reactivate()
    {
        // Reset any other states before reactivating
        if (GetComponent<Platform>())
        {
            GetComponent<Platform>().ResetState();
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;
        gameObject.SetActive(true);
    }

    public void UntilNextSet()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void Temporary(int spawnDelay)
    {
        Debug.Log("Temporary Spawn Called!");
        Instantiate(explosion, transform.position, Quaternion.identity);

        // Hide the object
        gameObject.SetActive(false);

        // Schedule reactivation
        Invoke("Reactivate", spawnDelay);
    }

    public void Permanent()
    {
        if (GetComponent<Platform>())
        {
            Debug.Log("Permanent explosion triggered");
        }

        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        Instantiate(explosion, transform.position, Quaternion.identity);

        if (GetComponentInParent<Remix>())
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
