using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    public bool randomDrift = false;
    private float mass;
    private Vector3 startPosition;
    private Vector3 destination;
    private float amount;
    // Use this for initialization

    void Start()
    {
        mass = Random.Range(.01f, .05f);
        startPosition = transform.position;
        if (randomDrift)
        {
            destination = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
            amount = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (randomDrift)
        {
            transform.position = Vector3.Lerp(startPosition, destination, amount);

            amount += .001f;
            if (amount >= 1f)
            {
                startPosition = destination;
                destination = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
                amount = 0f;
            }
        }
        else
        {
            Vector3 topLeftBoundary = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane / 2));
            Vector3 bottomRightBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.farClipPlane / 2));
            Vector3 xPos = new Vector3(Input.acceleration.x * mass, 0, 0);
//            Vector3 xPos = new Vector3(Input.GetAxis("Mouse X") * mass, 0, 0); ;
                      Vector3 yPos = new Vector3(0, Input.acceleration.y * mass, 0);
 //           Vector3 yPos = new Vector3(0, Input.GetAxis("Mouse Y") * mass, 0);
            if ((transform.position.x > topLeftBoundary.x && xPos.x < 0) || (transform.position.x < bottomRightBoundary.x && xPos.x > 0))
            {
                transform.position = transform.position + xPos;
            }

            if((transform.position.y > topLeftBoundary.y && yPos.y > 0) || (transform.position.y < bottomRightBoundary.y && yPos.y < 0))
            {
                transform.position = transform.position + yPos;
            }



              if(transform.position.x <= topLeftBoundary.x || transform.position.x >= bottomRightBoundary.x || transform.position.y <= topLeftBoundary.y || transform.position.y >= bottomRightBoundary.y)
              {
               // Debug.Log("Off X Screen");
                //off screen
                Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
                transform.position = screenPosition;
            }
        }
    }
}
