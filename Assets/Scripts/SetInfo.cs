using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectRespawn
{
    public GameObject respawnedObject;
    public float timeUntilActive;
}
public class SetInfo : MonoBehaviour
{

    public GameObject spawnLocations;
    public GameObject platforms;
    public Loot[] availableLoot;
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

    //TODO: Set Weight

    public int weight = 10;


    //remaining spaces to populate

    // Start is called before the first frame update


    void Start()
    {
        objectsToRespawn = new List<ObjectRespawn>();

        Vector3 topOfScreen = platforms.transform.position;
//        topOfScreen.y = 10f;
//        platforms.transform.position = topOfScreen;
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
//            platformsToMove[i].transform.localScale = Vector3.zero;          
        }
        currentSet = gameObject.GetComponent<ProceduralInfo>();
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
//shouldn't need this a nymore
//        transform.position = Vector3.zero;
        gameObject.SetActive(false);

    }

    void Update()
    {
        int currentNumberOfPlatformsToScale = 0;
        Vector3 velocity = Vector3.one;
        if (movingOffScreenInProgress)
        {

            Platform[] platformsToScaleDown = platforms.GetComponentsInChildren<Platform>();
//            int numberOfPlatformsToScaleDown = platformsToScaleDown.Length;
            for (int i = 0; i < platformsToScaleDown.Length; i++)
            {
                if(platformsToScaleDown[i].GetComponent<Explode>())
                {
                    platformsToScaleDown[i].GetComponent<Explode>().Temporary(1);
                }
            }
            PlatformsFinishedMovingOffScreen();

        }

        if (movingOnScreenInProgress) {
            Platform[] platformsToScaleUp = platforms.GetComponentsInChildren<Platform>();
                int numberOfPlatformsToScaleUp = platformsToScaleUp.Length;

                currentNumberOfPlatformsToScale++;
                for (int i = 0; i < platformsToScaleUp.Length; i++) {
                    platformsToScaleUp[i].transform.localScale = Vector3.Lerp(platformsToScaleUp[i].transform.localScale, platformsToScaleUp[i].originalScale, .1f);

                if (platformsToScaleUp[i].transform.localScale == platformsToScaleUp[i].originalScale)
                    {
                        //finished scaling up
                        platformsToScaleUp[i].scaledUp = true;
                        platformsToScaleUp[i].scaledDown = false;
                        currentNumberOfPlatformsToScale++;
                    }
                }

                if (currentNumberOfPlatformsToScale >= numberOfPlatformsToScaleUp)
                {
                    Debug.Log("Platforms Finished Moving On Screen");
                    PlatformsFinishedMovingOnScreen();
                }
        }

        for (int i = 0; i < objectsToRespawn.Count; i++)
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
