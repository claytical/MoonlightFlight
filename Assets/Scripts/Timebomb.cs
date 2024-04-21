using UnityEngine;

public class Timebomb : MonoBehaviour
{
    public float destructionDelay = 5f; // Time in seconds before destruction
    public bool scaleOverTime = false; // Whether the object should grow in scale over time
    public float growthRate = 0.1f; // Rate at which the object grows per second

    private Vector3 initialScale; // Initial scale of the object
    private float rotationSpeed = 360f; // Rotation speed in degrees per second

    void Start()
    {
        // Start the destruction countdown
        Invoke("DestroyObject", destructionDelay);

        // Store the initial scale of the object
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Scale the object over time if enabled
        if (scaleOverTime)
        {
            // Calculate the new scale based on growth rate
            Vector3 newScale = transform.localScale + Vector3.one * growthRate * Time.deltaTime;
            // Apply the new scale
            transform.localScale = newScale;
        }

        // Rotate the object by 360 degrees over the countdown period
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationAmount);
    }

    void DestroyObject()
    {
        // Destroy the GameObject
        Destroy(gameObject);
    }
}
