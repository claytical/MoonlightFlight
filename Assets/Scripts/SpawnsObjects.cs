using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsObjects : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float timeBetweenSpawns = 5f;
    public float spawnedObjectLifetime = 10f; // Time in seconds before the spawned object is destroyed
    public GameObject spawningParticles;

    private ParticleSystem particles;
    private float nextSpawnTime;
    private GameObject so;

    // Start is called before the first frame update
    void Start()
    {
        if (spawningParticles)
        {
            spawningParticles = Instantiate(spawningParticles, transform);
            spawningParticles.transform.position = transform.position;
            particles = spawningParticles.GetComponent<ParticleSystem>();
        }

        if (objectToSpawn && timeBetweenSpawns > 0)
        {
            // Set the initial spawn time
            nextSpawnTime = Time.time + timeBetweenSpawns;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeBetweenSpawns > 0 && Time.time >= nextSpawnTime)
        {
            // Spawn the object if the spawn time is met and no object is currently active
            if (!so)
            {
                SpawnObject();
            }

            // Set the next spawn time
            nextSpawnTime = Time.time + timeBetweenSpawns;
        }
    }

    public void StartParticles()
    {
        if (particles)
        {
            particles.Play();
        }
    }

    public void SpawnObject()
    {
        if (particles)
        {
            particles.Stop();
        }

        // Create a new object
        so = Instantiate(objectToSpawn, transform.parent);
        so.transform.position = transform.position;

        // Add the SpawnedObject component and set its lifetime
        SpawnedObject spawnedObject = so.AddComponent<SpawnedObject>();
        spawnedObject.SetLifeTime(spawnedObjectLifetime, gameObject);

        Debug.Log("New object spawned: " + so.name);

        // Reactivate particles if needed
        StartParticles();
    }

    // Optionally, handle object collection or destruction
    public void OnObjectCollectedOrDestroyed()
    {
        so = null; // Reset the reference to allow spawning the next object
    }
}
