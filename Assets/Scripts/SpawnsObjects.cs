using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsObjects : MonoBehaviour
{
    public bool collisionCausesSpawn = false;
    public float timeBetweenSpawns;
    public GameObject timeCircle;
    public float spawnedObjectLifetime;
    public int numberOfSpawnsBeforeSelfDestruct;
    public GameObject[] objectsToSpawn;
    public GameObject spawnPoint;
    public Vector2 velocity;

    private Color originalIdentifierColor;
    private List<GameObject> spawnedObjects;
    private float nextSpawnTime;
    private int spawnedObjectIndex = 0;
    private int numberOfObjectsSpawned = 0;
    private Vector3 spawnTimerSpawnPosition;
    private Color flashColor;
    private Color standardColor;

    // Start is called before the first frame update
    void Start()
    {


        if (!spawnPoint)
        {
            spawnPoint = this.gameObject;
        }
        spawnedObjects = new List<GameObject>();
        if (objectsToSpawn.Length > 0 && timeBetweenSpawns > 0)
        {
            nextSpawnTime = Time.time + timeBetweenSpawns;
            SetNextSpawnedItem();

        }
        if (timeCircle)
        {
            spawnTimerSpawnPosition = timeCircle.transform.position;
        }


    }
    public GameObject NextSpawnedObject()
    {
        return objectsToSpawn[spawnedObjectIndex];
    }

    void SetNextSpawnedItem()
    {
        GetComponent<Remix>().identifier.sprite = objectsToSpawn[spawnedObjectIndex].GetComponent<Standard>().icon;
        if (objectsToSpawn[spawnedObjectIndex].GetComponent<Hazard>())
        {
            Debug.Log("Has hazard component...");
            GetComponent<Remix>().identifier.color = GetComponent<Remix>().GetHazardColor();
        }
        else 
        {
            GetComponent<Remix>().identifier.color = GetComponent<Remix>().GetOriginalIdentifierColor();
        }
        if(GetComponent<Remix>())
        {
            GetComponent<Remix>().identifier.gameObject.transform.rotation = Quaternion.identity;
         if(objectsToSpawn.Length < spawnedObjectIndex)
            {
                GetComponent<Remix>().identifier.flipY = objectsToSpawn[spawnedObjectIndex].GetComponent<Remix>().identifier.flipY;
                GetComponent<Remix>().identifier.flipX = objectsToSpawn[spawnedObjectIndex].GetComponent<Remix>().identifier.flipX;

            }

        }


    }

    // Update is called once per frame
    void Update()
    {
        if (timeBetweenSpawns > 0)
        {
            float timeLeft = nextSpawnTime - Time.time;

            Vector3 circlePos = transform.position;
            float rotatedAngle = Mathf.Deg2Rad * 90;// -Mathf.PI;// * 1.5f;
            rotatedAngle += Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
            float scalar = .7f;

            //Spawning Up
            /*
            circlePos.y = circlePos.y + ( (Mathf.Sin(2f * Mathf.PI * (timeLeft / timeBetweenSpawns) + (Mathf.PI / 2)) * .7f));
            circlePos.x = circlePos.x + ( (Mathf.Cos(2f * Mathf.PI * (timeLeft / timeBetweenSpawns) + (Mathf.PI / 2)) * .7f));
            */
            //Spawning Down?
//            circlePos.y = circlePos.y + ( (Mathf.Sin (2f * Mathf.PI * (timeLeft / timeBetweenSpawns) + (Mathf.PI * 1.5f)) * .7f));
//            circlePos.x = circlePos.x + ( (Mathf.Cos (2f * Mathf.PI * (timeLeft / timeBetweenSpawns) + (Mathf.PI * 1.5f)) * .7f));

            circlePos.y = circlePos.y + ((Mathf.Sin(2f * Mathf.PI * (timeLeft / timeBetweenSpawns) + (rotatedAngle)) * .7f));

            circlePos.x = circlePos.x + ((Mathf.Cos(2f * Mathf.PI * (timeLeft / timeBetweenSpawns) + (rotatedAngle)) * scalar));



            //            float distance = Vector3.Distance(circlePos, spawnTimerSpawnPosition);
            timeCircle.transform.position = circlePos;

        }
    }

    void FixedUpdate()
    {
        if (timeBetweenSpawns > 0 && nextSpawnTime <= Time.time)
        {
            nextSpawnTime = Time.time + timeBetweenSpawns;
            SpawnObject();
        }


    }

    private void turnOnCollision()
    {
        GetComponent<Platform>().TurnOnCollision();
    }

    public void SpawnObject()
    {
        if(GetComponent<Animator>())
        {
            GetComponent<Animator>().SetTrigger("hit");
        }
        if (numberOfObjectsSpawned <= numberOfSpawnsBeforeSelfDestruct)
            {
                if(GetComponent<Platform>())
            {
                GetComponent<Platform>().TurnOffCollision();
                Invoke("turnOnCollision", 1);
            }
                GameObject go = Instantiate(objectsToSpawn[spawnedObjectIndex], spawnPoint.transform.position, objectsToSpawn[spawnedObjectIndex].transform.rotation, transform.parent);
                go.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.down);
                spawnedObjects.Add(go);
                SpawnedObject so = go.AddComponent<SpawnedObject>();
                so.parentCollider = GetComponent<Collider2D>();
                if (spawnedObjectLifetime > 0)
                {
                    so.SetLifeTime(spawnedObjectLifetime);

                }


            }


            if (numberOfObjectsSpawned > numberOfSpawnsBeforeSelfDestruct)
            {
                if (GetComponent<Explode>())
                {
                    Debug.Log("Blowing up platform...");
                    GetComponent<Explode>().Go();
                }
                else
            {
                Destroy(gameObject);

            }

        }


            spawnedObjectIndex++;
            numberOfObjectsSpawned++;
            if (spawnedObjectIndex >= objectsToSpawn.Length)
            {
                spawnedObjectIndex = 0;
            }
            SetNextSpawnedItem();

        }

}
