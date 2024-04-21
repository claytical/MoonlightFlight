using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsObjects : MonoBehaviour
{
    public bool collisionCausesSpawn = false;
    public float timeBetweenSpawns;
    public GameObject timeCircle;
    public GameObject spawnPop;
    public GameObject spawnerPop;
//    public GameObject timeStick;
    public float spawnedObjectLifetime;
    public int numberOfSpawnsBeforeSelfDestruct;
    public GameObject[] objectsToSpawn;
    public GameObject spawnPoint;
    public Vector2 velocity;
    
 //   private Color originalIdentifierColor;
    private List<GameObject> spawnedObjects;
    private float nextSpawnTime;
    private int spawnedObjectIndex = 0;
    private int numberOfObjectsSpawned = 0;
    private SpawnedObject so;
/*    private Vector3 spawnTimerSpawnPosition;
    private Color flashColor;
    private Color standardColor;
    private bool isDeactivated;
*/
    private float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if(spawnerPop.GetComponent<ParticleSystem>())
        {
            ParticleSystem ps = spawnerPop.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = ps.main;
            mainModule.duration = timeBetweenSpawns - (timeBetweenSpawns/2);
        }

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
    void Update()
    {
        /*
        if (timeBetweenSpawns > 0)
        {

            float rotationAmount = rotationSpeed * Time.deltaTime;
            timeStick.transform.Rotate(Vector3.forward, rotationAmount);
            float timeLeft = nextSpawnTime - Time.time;
        }
        */
    }

    void FixedUpdate()
    {
        if (timeBetweenSpawns > 0 && nextSpawnTime <= Time.time)
        {
            nextSpawnTime = Time.time + timeBetweenSpawns;
            Invoke("SpawnPop", timeBetweenSpawns - .5f);
            SpawnObject();
        } 



    }

    private void turnOnCollision()
    {
        GetComponent<Platform>().TurnOnCollision();
    }

    void ReactivateObject()
    {
        // Deactivate the GameObject
        gameObject.SetActive(false);
        Invoke("ActivateObject", .5f);
    }

    void SpawnerPop()
    {
        Invoke("ActivateSpawner", .1f);
        GameObject go = Instantiate(spawnerPop, transform.parent);
        go.transform.position = transform.position;
    }

    void ActivateSpawner()
    {
        gameObject.SetActive(true);
    }
    void ActivateObject()
    {
        if(so)
        {

            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            if (!circleCollider.enabled)
            {
                if (Vector2.Distance(transform.position, so.transform.position) < 1f)
                {
                    Invoke("ActivateObject", .1f);
                }
                else
                {
                    SpawnerPop();
                }
            }
        }

    }
    public void SpawnPop()
    {
        GameObject go = Instantiate(spawnPop, transform.parent);
        go.transform.position = transform.position;

    }

    public void SpawnObject()
    {

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