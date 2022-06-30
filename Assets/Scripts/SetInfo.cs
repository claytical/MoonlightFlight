﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ObjectRespawn
{
    public GameObject respawnedObject;
    public float timeUntilActive;
}
public class SetInfo : MonoBehaviour
{

    public GameObject spawnLocations;
    public GameObject platforms;
    public GameObject[] breakables;
    public List<ObjectRespawn> objectsToRespawn;
    public int numberOfObjectsToPlace;
    public int sets = 5;
    public ProceduralInfo currentSet;
    public float movingSpeed = .03f;
    private Vehicle vehicle;
    private Transform[] platformsToMove;
    private bool movingOffScreenInProgress = false;
    private bool movingOnScreenInProgress = false;

    //remaining spaces to populate

    // Start is called before the first frame update

    void Start()
    {
        Vector3 topOfScreen = platforms.transform.position;
        topOfScreen.y = 10f;
        platforms.transform.position = topOfScreen;
        platforms.SetActive(true);
        //add platforms to descending objects
        platformsToMove = platforms.GetComponentsInChildren<Transform>();


        for (int i = 0; i < platformsToMove.Length; i++)
        {
            if(platformsToMove[i].gameObject.GetComponent<BoxCollider2D>())
            {
//                platformsToMove[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (platformsToMove[i].gameObject.GetComponent<PolygonCollider2D>())
            {
            }

            movingOnScreenInProgress = true;
        }
        currentSet = gameObject.GetComponent<ProceduralInfo>();
        objectsToRespawn = new List<ObjectRespawn>();
    }

    public void SetVehicle(Vehicle v)
    {
        vehicle = v;
    }

    public void PlatformTransparency(bool transparent)
    {
        Debug.Log("Setting Transparency to " + transparent);
        Rigidbody2D [] bumpables = platforms.GetComponentsInChildren<Rigidbody2D>();

            for(int i = 0; i < bumpables.Length; i++)
            {
                if(bumpables[i].GetComponent<Animator>()) {
                    if (transparent)
                    {
                        bumpables[i].gameObject.GetComponent<Animator>().SetTrigger("transparent");
                    }
                    else
                    {
                        bumpables[i].gameObject.GetComponent<Animator>().SetTrigger("solid");
                        if (bumpables[i].gameObject.GetComponent<Remix>())
                        {
                            bumpables[i].gameObject.GetComponent<Remix>().SetColors();
                            bumpables[i].gameObject.GetComponent<Platform>().SetConstraints();
                            Debug.Log("Setting constraints and colors");
                        }
                        else
                        {
                            Debug.Log(bumpables[i].gameObject + " does not have remix script!");
                        }
                }

                if (bumpables[i].GetComponent<BoxCollider2D>())
                {
                    bumpables[i].GetComponent<BoxCollider2D>().enabled = !transparent;
                }
            }
        }
    }

    public void MovePlatformsOntoScreen()
    {

    }

    public void MovePlatformsOffScreen()
    {
        platformsToMove = platforms.GetComponentsInChildren<Transform>();
        for (int i = 0; i < platformsToMove.Length; i++)
        {
            if(platformsToMove[i].gameObject.GetComponent<BoxCollider2D>())
            {
                platformsToMove[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }

            if (platformsToMove[i].gameObject.GetComponent<PolygonCollider2D>()) {
                platformsToMove[i].gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            }

            if (platformsToMove[i].gameObject.GetComponent<CircleCollider2D>())
            {
                platformsToMove[i].gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }

        }
        movingOffScreenInProgress = true;
    }

    private void PlatformsFinishedMovingOnScreen()
    {

        movingOnScreenInProgress = false;
        transform.position = Vector3.zero;
        platformsToMove = platforms.GetComponentsInChildren<Transform>();
        for (int i = 0; i < platformsToMove.Length; i++)
        {
            if (platformsToMove[i].gameObject.GetComponent<BoxCollider2D>())
            {
                platformsToMove[i].gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
            if (platformsToMove[i].gameObject.GetComponent<PolygonCollider2D>())
            {
                platformsToMove[i].gameObject.GetComponent<PolygonCollider2D>().enabled = true;
            }

        }
        if(GetComponent<OnboardComputer>())
        {
            GetComponent<OnboardComputer>().tutorial.SetActive(true);
        }


    }
    private void PlatformsFinishedMovingOffScreen()
    {
        movingOffScreenInProgress = false;
        transform.position = Vector3.zero;
        gameObject.SetActive(false);

    }

    void Update()
    {
        if(movingOffScreenInProgress)
        {
            Vector3 nextPosition = platforms.transform.position;
            nextPosition.y -= movingSpeed;

            platforms.transform.position = nextPosition;
            if(platforms.transform.position.y <= -10f)
            {

                PlatformsFinishedMovingOffScreen();
            }
        }

        if(movingOnScreenInProgress) {
            Vector3 nextPosition = platforms.transform.position;
            nextPosition.y -= movingSpeed;

            platforms.transform.position = nextPosition;
            if (platforms.transform.position.y <= 0)
            {
                Debug.Log("Platforms Finished Moving On Screen");
                PlatformsFinishedMovingOnScreen();
            }

        }

        for(int i = 0; i < objectsToRespawn.Count; i++)
        {
            if(objectsToRespawn[i].timeUntilActive <= Time.time)
            {
                objectsToRespawn[i].respawnedObject.SetActive(true);
                objectsToRespawn.RemoveAt(i);
                break;
            }
        }

    }


    public void BrokeObject()
    {
        Debug.Log("BROKE OBJECT!");
    }

}
