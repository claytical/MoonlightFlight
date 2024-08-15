using UnityEngine;

public class PlayerStatsTracking : MonoBehaviour
{
    public int score = 0;
    public float distanceCovered = 0;
    public int obstaclesAvoided = 0;
    public int itemsCollected = 0;
    public int damageTaken = 0;
    public int assists = 0;
    public int fuelUsed = 0;

    private Vector3 lastPosition;

    void Start()
    {
        // Record the initial position to start tracking distance
        lastPosition = transform.position;
    }

    public void AddScore(int points)
    {
        score += points;
    }

    public void RecordObstacleAvoidance()
    {
        obstaclesAvoided++;
        AddScore(10);  // Example scoring
    }

    public void RecordItemCollected()
    {
        itemsCollected++;
        AddScore(5);  // Example scoring
    }

    public void RecordDamage(int damage)
    {
        damageTaken += damage;
        AddScore(-damage);  // Penalty for taking damage
    }

    public void RecordAssist()
    {
        assists++;
        AddScore(15);  // Example scoring
    }

    public void RecordFuelUsed(int amount)
    {
        fuelUsed += amount;
        AddScore(amount);  // Reward for managing fuel, adjust scoring logic as needed
    }

}
