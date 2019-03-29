using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    public bool randomDrift = false;
    private float angle = 0f;
    private float speed = 5f;
    private Vector3 startPosition;
    private Vector3 destination;
    private float amount;
    // Use this for initialization
    void Start()
    {
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
            if(transform.position.x > topLeftBoundary.x && transform.position.x < bottomRightBoundary.x)
            {
                Vector3 xPos = new Vector3(-Input.acceleration.x, 0, 0);
                transform.position = transform.position + xPos;
            }
            if (transform.position.y > topLeftBoundary.y && transform.position.y < bottomRightBoundary.y)
            {
                Vector3 yPos = new Vector3(0, -Input.acceleration.y, 0);
                transform.position = transform.position + yPos;
            }
        }
    }
}
