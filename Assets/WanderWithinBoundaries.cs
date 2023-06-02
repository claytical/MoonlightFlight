using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderWithinBoundaries : MonoBehaviour
{
    public Vector3 boundariesCenter;  // Center of the boundaries
    public Vector3 boundariesSize;    // Size of the boundaries

    public float wanderSpeed = 3f;    // Speed of the wandering

    private Vector3 targetPosition;   // Current target position for the GameObject

    private void Start()
    {
        // Calculate the initial target position within the boundaries
        targetPosition = GetRandomPositionWithinBoundaries();
        targetPosition *= .1f;
    }

    private void Update()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);

        // Check if the GameObject has reached the target position
        if (transform.position == targetPosition)
        {
            // Calculate a new random target position within the boundaries
            targetPosition = GetRandomPositionWithinBoundaries();
        }
    }

    private Vector3 GetRandomPositionWithinBoundaries()
    {
        // Calculate random coordinates within the boundaries
        float minX = boundariesCenter.x - boundariesSize.x / 2f * transform.localScale.x;
        float maxX = boundariesCenter.x + boundariesSize.x / 2f * transform.localScale.x;
        float minY = boundariesCenter.y - boundariesSize.y / 2f * transform.localScale.y;
        float maxY = boundariesCenter.y + boundariesSize.y / 2f * transform.localScale.y;
        float minZ = boundariesCenter.z - boundariesSize.z / 2f * transform.localScale.z;
        float maxZ = boundariesCenter.z + boundariesSize.z / 2f * transform.localScale.z;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = Random.Range(minZ, maxZ);

        // Return the random position within the boundaries
        return new Vector3(randomX, randomY, randomZ);
    }
}
