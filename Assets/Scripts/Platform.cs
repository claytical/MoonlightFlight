using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool finished = false;
    private Vector3 originalScale;
    public bool canBePushed = false;
    public bool canTurnHazardous = false;
    public bool canSpawnNewObjects;
    public float spawnTimer = 0;
    private float nextSpawnTime;
    public int numberOfSpawnedObjectsBeforeSelfDestruction = -1;
    public int maximumNumberOfSpawnedObjects = -1;
    public int spawnedObjectLifetime = -1;
    private int numberOfObjectsSpawned = 0;
    public GameObject[] objectsToSpawn;
    private int spawnedObjectIndex = 0;
    public Transform[] placesToSpawn;
    public List<GameObject> spawnedObjects;
    public GameObject explosion;
    public RigidbodyConstraints2D constraints;
    public float gravity;

    private bool isMovingFromPush = false;
    private Vector3 originalPosition;
    private Vector3 pushToPosition;

    // Start is called before the first frame update
    void Start()
    {
        spawnedObjects = new List<GameObject>();
        if (spawnTimer > 0)
        {
            nextSpawnTime = Time.time + spawnTimer + 3;
            SetNextSpawnedItem();

        }

        originalScale = transform.localScale;
        GetComponent<BoxCollider2D>().enabled = false;
        if (canBePushed)
        {
            originalPosition = transform.position;
        }
        SetConstraints();
    }

    void SetNextSpawnedItem()
    {
        GetComponent<Remix>().identifier.sprite = objectsToSpawn[spawnedObjectIndex].GetComponent<Remix>().identifier.sprite;
        Debug.Log("Changing " + GetComponent<Remix>().identifier.gameObject.name + " to " + objectsToSpawn[spawnedObjectIndex].GetComponent<Remix>().identifier.sprite.name);

        GetComponent<Remix>().identifier.gameObject.transform.rotation = Quaternion.identity;
    }
    void FixedUpdate()
    {
        if (spawnTimer > 0 && nextSpawnTime <= Time.time)
        {
            nextSpawnTime = Time.time + spawnTimer;
            Debug.Log("NEXT SPAWN TIME: " + nextSpawnTime);
            SpawnObject();
        }
    }

    private void switchTag()
    {
        gameObject.tag = "Avoid";

    }
    public void TurnHazardous()
    {
        Invoke("switchTag", 1);
        //TODO: Change to custom sprite
        GetComponent<Remix>().identifier.enabled = false;
        GetComponent<Remix>().identifier.transform.localScale = Vector3.one;
        GetComponent<Animator>().enabled = false;
        Debug.Log("turned hazardous");
    }

    // Update is called once per frame

    public void SpawnObject()
    {
        Vector3 newPosition = transform.position;
        //        newPosition.y += -1;

        GameObject go = Instantiate(objectsToSpawn[spawnedObjectIndex], transform.position, transform.rotation, transform.parent);
        go.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.down);
        spawnedObjectIndex++;
        if(spawnedObjectIndex >= objectsToSpawn.Length)
        {
            spawnedObjectIndex = 0;
        }
        numberOfObjectsSpawned++;
        spawnedObjects.Add(go);
        SetNextSpawnedItem();

        if (numberOfObjectsSpawned > maximumNumberOfSpawnedObjects)
        {
            Debug.Log("Culling spawned objects...");
            Vector3 pos = transform.position;
            Instantiate(spawnedObjects[0].GetComponent<Platform>().explosion, spawnedObjects[0].transform.position,Quaternion.identity);
            spawnedObjects[0].GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            spawnedObjects[0].gameObject.GetComponent<Animator>().SetTrigger("destroy");
            spawnedObjects.RemoveAt(0);
        }

        SpawnedObject so = go.AddComponent<SpawnedObject>();
        if (spawnedObjectLifetime > 0)
        {
            so.SetLifeTime(spawnedObjectLifetime);

        }

        if (numberOfObjectsSpawned >= numberOfSpawnedObjectsBeforeSelfDestruction && numberOfSpawnedObjectsBeforeSelfDestruction > 0)
        {
            Instantiate(explosion);
            Destroy(gameObject);
        }
    }



    void Update()
    {

    }

    public void SetConstraints()
    {
        GetComponent<Rigidbody2D>().constraints = constraints;
        GetComponent<Rigidbody2D>().gravityScale = gravity;
    }

    public void Disappear() {
        Destroy(this.gameObject);
    }
    public void Finished()
    {
        //CALLED IN INITIAL ANIMATION
        finished = true;
        transform.localScale = originalScale;
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
