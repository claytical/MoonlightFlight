using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    /*    public bool randomGravity;
        public float timeUntilDestroy;
    */
    public enum EnemyType
    {
        SIMPLETON = 0,
        SEEKER = 1,
        DRIFTER = 2,
        DRONE = 3,
        ANOMALY = 4,
        CHARISMATIC = 5,
        TIMEBOMB = 6,
        BOUNDARY = 7

    };

    public EnemyType hazardType;
    public int damage;
    public bool drift;
    public float seekerSpeed;
    public float scaleSpeed = .1f;


    private Vector2 driftDirection;
    private bool seekingVehicles = false;
    private int vehicleIndex = -1;
    private Color color;
    private float scaleDirection = -1;
    private bool scaling = false;
    private Vector3 originalScale;
    private Player[] players;
    // Use this for initialization
    void Start()
    {
        switch (hazardType)
        {
            case EnemyType.SEEKER:
            case EnemyType.CHARISMATIC:
                vehicleIndex = FindVehicle();
                break;
            case EnemyType.ANOMALY:
                //Rigidbody gravity scale is 1 or -1
                break;
            case EnemyType.DRIFTER:
                driftDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                break;
            case EnemyType.DRONE:
                break;
            case EnemyType.SIMPLETON:
                break;
        }

    }

    private int FindVehicle()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        players = new Player[gos.Length];

        for (int i = 0; i < gos.Length; i++)
        {
            players[i] = gos[i].GetComponent<Player>();
        }

        if(players.Length > 0)
        {
            int index = Random.Range(0, players.Length);
            if(players[index].chosenVehicle)
            {
                if (players[index].chosenVehicle.GetComponent<Vehicle>().isFlying())
                {
                    return index;
                }
            }
            else
            {
                return -1;
            }
        }
        return -1;
    }

    private void Seekers()
    {
        if (vehicleIndex >= 0)
        {
            seekingVehicles = true;
        }

        if (seekingVehicles)
        {
            if (players.Length > 0 && vehicleIndex >= 0)
            {
                if(players[vehicleIndex].chosenVehicle)
                {
                    Drift(players[vehicleIndex].chosenVehicle.transform.position);
                }
                else
                {
                    FindVehicle();
                }
            }
            else
            {
                Debug.Log("Need new vehicle to track");
            }
        }

    }


    private void FixedUpdate()
    {
        if (vehicleIndex == -1)
        {
            vehicleIndex = FindVehicle();
        }

        switch (hazardType)
        {
            case EnemyType.SEEKER:
                Seekers();
                break;
            case EnemyType.ANOMALY:
                transform.parent.position = transform.position;
                transform.parent.rotation = transform.rotation;
                break;
            case EnemyType.CHARISMATIC:                
                players[vehicleIndex].chosenVehicle.GetComponent<Vehicle>().Drift(transform.position);

                break;
            case EnemyType.DRIFTER:
                GetComponent<Rigidbody2D>().AddForce(driftDirection);
                break;
            case EnemyType.DRONE:
                break;
            case EnemyType.SIMPLETON:
                break;
        }

        if (GetComponentInParent<Platform>())
        {
            transform.parent.position = transform.position;
        }
    }

    public void Drift(Vector3 position)
    {
        if (GetComponent<Rigidbody2D>())
        {
            Vector3 direction = (position - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(direction * seekerSpeed, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("HAZARD COLLISION! with " + coll.gameObject.name);
        
        if(GetComponentInParent<Platform>())
        {
            if(!GetComponentInParent<Platform>().indestructable)
            {
                Debug.Log("Parent should blow up.");
                GetComponent<Explode>().Permanent();
            }
        }
        
        if(coll.gameObject.GetComponent<Vehicle>())
        {
            //HAZARD HIT VEHICLE
            if (coll.gameObject.GetComponentInParent<Player>().TakeDamage(damage))
            {
                coll.gameObject.GetComponentInParent<Player>().Restart();
                


                //EXPLODE
                if (coll.gameObject.GetComponent<Explode>())
                {
                    coll.gameObject.GetComponent<Explode>().Permanent();

                }
                else
                {
                    coll.gameObject.GetComponent<Rigidbody2D>().AddExplosionForce(10f, transform.position, 10f);
                }
            }
            
            if (GetComponent<Explode>())
            {
                GetComponent<Explode>().Permanent();
            }
        }

        //        scaling = true;
        if (GetComponent<SpriteRenderer>())
        {
            }

    }

}

