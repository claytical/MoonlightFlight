using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsObjects : MonoBehaviour
{
    public float timeBetweenSpawns;
    public GameObject spawnPop;
    public float spawnedObjectLifetime;
    public int numberOfSpawnsBeforeSelfDestruct;
    public GameObject[] objectsToSpawn;
    public GameObject spawnPoint;
    public Vector2 velocity;
    
    private List<GameObject> spawnedObjects;
    private float nextSpawnTime;
    private int spawnedObjectIndex = 0;
    private int numberOfObjectsSpawned = 0;
    private SpawnedObject so;
    private float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if(timeBetweenSpawns > 0)
        {
            float rotationDistance = 360f;
            rotationSpeed = rotationDistance / timeBetweenSpawns;
        }

        spawnedObjects = new List<GameObject>();

        if (objectsToSpawn.Length > 0 && timeBetweenSpawns > 0)
        {
            nextSpawnTime = Time.time + timeBetweenSpawns;
        }

        Invoke("SpawnPop", timeBetweenSpawns - .5f); 

    }

    public GameObject NextSpawnedObject()
    {
        return objectsToSpawn[spawnedObjectIndex];
    }

    void SetNextSpawnedItem()
    {
        if (objectsToSpawn[spawnedObjectIndex].GetComponent<Hazard>())
        {
            Debug.Log("Has hazard component...");
            GetComponent<Remix>().identifier.color = GetComponent<Remix>().GetHazardColor();
        }
        else
        {
            GetComponent<Remix>().identifier.color = GetComponent<Remix>().GetOriginalIdentifierColor();
        }
        if (GetComponent<Remix>())
        {
            GetComponent<Remix>().identifier.gameObject.transform.rotation = Quaternion.identity;
            if (objectsToSpawn.Length < spawnedObjectIndex)
            {
                GetComponent<Remix>().identifier.flipY = objectsToSpawn[spawnedObjectIndex].GetComponent<Remix>().identifier.flipY;
                GetComponent<Remix>().identifier.flipX = objectsToSpawn[spawnedObjectIndex].GetComponent<Remix>().identifier.flipX;

            }

        }


    }

    // Update is called once per frame
  
    void FixedUpdate()
    {
        if (timeBetweenSpawns > 0 && nextSpawnTime <= Time.time)
        {
            Debug.Log("Spawn Pop Invoked, Spawning Object");

            nextSpawnTime = Time.time + timeBetweenSpawns;
            Invoke("SpawnPop", timeBetweenSpawns - .5f);
            SpawnObject();
        } 



    }

    void ReactivateObject()
    {
        Debug.Log("Reactivating Object");
        // Deactivate the GameObject
        gameObject.SetActive(false);
        Invoke("ActivateObject", .5f);
    }

    void ActivateSpawner()
    {
        gameObject.SetActive(true);
    }
    void ActivateObject()
    {
        gameObject.SetActive(true);
/*
        Debug.Log("Activating Object");
        if(so)
        {
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            if (!circleCollider.enabled)
            {
                if (Vector2.Distance(transform.position, so.transform.position) < 1f)
                {
                    Debug.Log("Distance less than 1");
                }
                else
                {
                    Debug.Log("Distance greater than 1");
                }
            }
            else
            {
                Debug.Log("Circle Collider Enabled");
            }
        }
*/
    }
    public void SpawnPop()
    {
        GameObject go = Instantiate(spawnPop, transform.parent);
        go.transform.position = transform.position;

    }

    public void SpawnObject()
    {
        Debug.Log("Spawning Object");
        if (numberOfObjectsSpawned <= numberOfSpawnsBeforeSelfDestruct)
        {
            if (GetComponent<CircleCollider2D>())
            {
                GetComponent<CircleCollider2D>().enabled = false;
            }
            
            GameObject go = Instantiate(objectsToSpawn[spawnedObjectIndex], transform.parent);
            go.transform.position = transform.position;
            spawnedObjects.Add(go);
            so = go.AddComponent<SpawnedObject>();
            
            if (spawnedObjectLifetime > 0)
            {
                so.SetLifeTime(spawnedObjectLifetime);
            }
            
            ReactivateObject();
        }
    }
}