using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnsObjects : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float timeBetweenSpawns;
    public float spawnedObjectLifetime;
    public GameObject spawningParticles;

    private ParticleSystem.MainModule main;
    private ParticleSystem particles;
    private float nextSpawnTime;
    private GameObject so;
    private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {

        spawningParticles = Instantiate(spawningParticles, transform);
        spawningParticles.transform.position = transform.position;
        particles = spawningParticles.GetComponent<ParticleSystem>();

        if (objectToSpawn && timeBetweenSpawns > 0)
        {
            //SET THE NEXT TIMER FOR AN OBJECT TO APPEAR - 5 SECONDS
            nextSpawnTime = Time.time + timeBetweenSpawns;

        }

    }

    // Update is called once per frame
  
    void FixedUpdate()
    {
        if (timeBetweenSpawns > 0 && nextSpawnTime <= Time.time)
        {
            particles.Stop();
            //SPAWN TIME MET, SET NEXT TIME
            nextSpawnTime = Time.time + timeBetweenSpawns;

            //IS A SPAWNED OBJECT STILL ATTACHED TO THE SPAWNER?
            if (!so)
            {
                //SPAWN A NEW OBJECT
                SpawnObject();
            }
        } 



    }

    public void StartParticles()
    {
        particles.Play();
    }

    public void SpawnObject()
    {

        //TURN OFF PARTICLES
        particles.Stop();

        //CREATE NEW OBJECT
        GameObject go = Instantiate(objectToSpawn, transform.parent);
        go.transform.position = transform.position;
        go.AddComponent<SpawnedObject>();
        //won't move away from spawn point, this keeps the object visible until it's collected.
        go.GetComponent<SpawnedObject>().SetLifeTime(99999, gameObject);
        so = go;

        if (!go.GetComponentInChildren<Rigidbody2D>())
        {
            //IF SPAWNED OBJECT MOVES FROM SPAWN POINT...
        }

        Debug.Log("NEW OBJECT ASSIGNED TO SO" + so.name);
    //    ReactivateObject();
    }

}
