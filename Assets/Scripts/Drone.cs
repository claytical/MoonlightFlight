using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public List<Transform> targetPositions;
    public float moveSpeed = 5f;
    public bool moveBackAndForth = false;
    public float stopDelay = 1f; // Time in seconds to stop at each position

    private Rigidbody2D rb;
    private Transform currentTarget;
    private int currentIndex = 0;
    private int direction = 1; // 1 for forward, -1 for backward
    private bool isWaiting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set the first target position
        if (targetPositions.Count > 0)
        {
            currentTarget = targetPositions[0];
        }
    }

    void Update()
    {
        // Check if there are target positions
        if (targetPositions.Count == 0)
        {
            Debug.LogWarning("No target positions assigned!");
            return;
        }

        // Move towards the current target position if not waiting
        if (currentTarget != null && !isWaiting)
        {
            Vector2 directionVector = currentTarget.position - transform.position;
            rb.velocity = directionVector.normalized * moveSpeed;

            // Check if the object has reached the current target position
            if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                // If move back and forth is enabled, reverse direction when reaching end points
                if (moveBackAndForth)
                {
                    if (currentIndex == 0 || currentIndex == targetPositions.Count - 1)
                    {
                        direction *= -1;
                    }
                }

                // Move to the next target position based on direction
                currentIndex += direction;
                currentIndex = Mathf.Clamp(currentIndex, 0, targetPositions.Count - 1);
                currentTarget = targetPositions[currentIndex];

                // Start waiting
                StartCoroutine(WaitForNextPosition());
            }
        }
    }

    IEnumerator WaitForNextPosition()
    {
        isWaiting = true;
        yield return new WaitForSeconds(stopDelay);
        isWaiting = false;
    }
}
