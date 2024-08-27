using UnityEngine;

public class PlayerStatsTracking : MonoBehaviour
{
    public int score = 0;
    public float distanceCovered = 0f;
    public int obstaclesAvoided = 0;
    public int itemsCollected = 0;
    public int damageTaken = 0;
    public int assists = 0;
    public int fuelUsed = 0;
    public float timeTaken;
    public int maxEnergy;  // Example value
    public float maxTime;
    public float effiencyScore;

    private Vector3 lastPosition;

    void Start()
    {
        // Record the initial position to start tracking distance
        lastPosition = transform.position;
    }

    void Update()
    {
        // Update the distance covered by the player
        TrackDistance();
    }

    private void TrackDistance()
    {
        // Calculate the distance moved since the last frame
        float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);
        distanceCovered += distanceThisFrame;

        // Update the last position to the current position
        lastPosition = transform.position;
    }

    public void AddScore(int points)
    {
        score += points;
        // You might want to add a UI update here to reflect the new score
    }

    public void RecordObstacleAvoidance()
    {
        obstaclesAvoided++;
        AddScore(10);  // Example scoring, adjust as needed
    }

    public void RecordItemCollected()
    {
        itemsCollected++;
        AddScore(5);  // Example scoring, adjust as needed
    }

    public void RecordDamage(int damage)
    {
        damageTaken += damage;
        AddScore(-damage);  // Penalty for taking damage, adjust as needed
    }

    public void RecordAssist()
    {
        assists++;
        AddScore(15);  // Example scoring, adjust as needed
    }

    public void RecordFuelUsed(int amount)
    {
        fuelUsed += amount;
        AddScore(amount);  // Reward for managing fuel, adjust as needed
    }

    public float CalculateEfficiencyScore()
    {
        maxEnergy = GamepadManager.Instance.MaxItemsCollected();
        if (timeTaken == 0) return 0;  // Avoid division by zero
        effiencyScore = (itemsCollected / (float)maxEnergy) * (maxTime / timeTaken);
        return effiencyScore;
    }

}
