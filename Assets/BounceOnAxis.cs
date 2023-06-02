using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOnAxis : MonoBehaviour
{
    public float bounceSpeed = 5f;  // Speed of the bouncing
    public float scale = 1f;
    private float startY;           // Initial y-position of the GameObject

    private void Start()
    {
        startY = transform.position.y;  // Store the initial y-position
    }

    private void Update()
    {
        // Calculate the new y-position based on a sine wave
        float newY = startY + Mathf.Sin(Time.time * bounceSpeed) * scale;

        // Update the GameObject's position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
