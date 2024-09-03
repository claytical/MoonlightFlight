using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct BreakableInfo
{
    public GameObject breakable;  // The breakable prefab
    public int weight;  // The weight associated with this breakable
}

public class SetInfo : MonoBehaviour
{
    public Transform AutoSpawnLocation;
    public bool autoPopulateSpawnLocations = true;
    public bool spawnEverything = false;
    private Transform[] spawnLocations;
    public GameObject lootLocation;
    public Transform[] lootLocations;
    public GameObject platforms;  // Single GameObject that holds all platform-related objects
    public BreakableInfo[] breakableInfos;  // Array of BreakableInfo to handle weighted breakable spawning
    public int timesToRepeat = 5;  // Number of times to repeat spawning breakables
    public int weight = 1;  // Weight of this set for weighted random selection
    public ProceduralLevel proceduralLevel;  // Reference to ProceduralLevel
    public float movingSpeed = .03f;
    private int spawnedBreakablesCount = 0;
    private int maxBreakablesToSpawn;
    private bool initialized = false;  // Flag to check if initialization is complete

    private bool hasBeenCleared = false;  // Used to track if the set has been cleared


    void Start()
    {
        Transform[] spawnFilter = AutoSpawnLocation.GetComponentsInChildren<Transform>();
        int zeroCount = 0;
        spawnLocations = spawnFilter.Where(t => t.position != Vector3.zero).ToArray();
        if (lootLocation)
        {
            lootLocations = lootLocation.GetComponentsInChildren<Transform>();
        }

        platforms.SetActive(true);  // Make sure platforms are active initially
        Platform[] platformArray = platforms.GetComponentsInChildren<Platform>();
        proceduralLevel = FindFirstObjectByType<ProceduralLevel>();

        for (int i = 0; i < platformArray.Length; i++)
        {
            platformArray[i].SetColors(proceduralLevel.remix);
        }

        // Calculate the maximum number of breakables to spawn
        maxBreakablesToSpawn = (spawnLocations.Length/4) * timesToRepeat;
        proceduralLevel.SetBreakables(maxBreakablesToSpawn);
        initialized = true;  // Mark initialization as complete
    }
    public bool IsInitialized()
    {
        return initialized;
    }

    public void SpawnNextBatchOfBreakables()
    {
        if (!initialized)
        {
            Debug.LogWarning("SpawnNextBatchOfBreakables called before initialization was complete.");
            return;
        }

        if (spawnedBreakablesCount >= maxBreakablesToSpawn)
        {
            Debug.Log("SPAWNED BREAKABLES COUNT: " + spawnedBreakablesCount + " MAX BREAKABLES: " + maxBreakablesToSpawn);
            return;
        }

        int breakablesInThisBatch = Random.Range(1, 2);  // Spawn 1 to 2 breakables in each batch
        for (int i = 0; i < breakablesInThisBatch && spawnedBreakablesCount < maxBreakablesToSpawn; i++)
        {
            int spawnIndex = spawnedBreakablesCount % spawnLocations.Length;
            GameObject breakableToSpawn = GetWeightedRandomBreakable();
            if (IsLocationClear(spawnLocations[spawnIndex].position))
            {
                GameObject go = Instantiate(breakableToSpawn, spawnLocations[spawnIndex].position, Quaternion.identity, transform);
                Debug.Log(go.name + " created at " + go.transform.position);
                spawnedBreakablesCount++;
            }
            else
            {
                Debug.Log("Skipped spawning at " + spawnLocations[spawnIndex].position + " due to vehicle presence.");
            }
        }

        Debug.Log($"Spawned {spawnedBreakablesCount}/{maxBreakablesToSpawn} breakables.");
    }

    private GameObject GetWeightedRandomBreakable()
    {
        int totalWeight = 0;

        // Calculate the total weight
        foreach (var breakableInfo in breakableInfos)
        {
            totalWeight += breakableInfo.weight;
        }

        int randomWeight = Random.Range(0, totalWeight);

        // Select a breakable based on the weighted random value
        foreach (var breakableInfo in breakableInfos)
        {
            if (randomWeight < breakableInfo.weight)
            {
                return breakableInfo.breakable;
            }
            randomWeight -= breakableInfo.weight;
        }

        // Fallback in case of any error, though this should never happen
        return breakableInfos[0].breakable;
    }

    public bool AllBreakablesSpawned()
    {
        return spawnedBreakablesCount >= maxBreakablesToSpawn;
    }

    public bool HasBeenCleared()
    {
        return hasBeenCleared;
    }

    public void SetCleared(bool cleared)
    {
        hasBeenCleared = cleared;
    }
    private bool IsLocationClear(Vector3 spawnPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 0.5f); // Adjust the radius as needed
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Vehicle>())
            {
                return false; // Location is not clear, a vehicle is present
            }
        }
        return true; // Location is clear
    }


    public void MoveOffScreen(Vector3 offScreenPosition, float duration)
    {
        Platform[] platformArray = platforms.GetComponentsInChildren<Platform>();

        foreach (Platform platform in platformArray)
        {
            if (platform != null)
            {
                // Perform operations on myObject
                platform.MoveOffScreen(offScreenPosition, duration);
            }

        }
    }

    public void ResetPlatforms()
    {
        Platform[] platformArray = platforms.GetComponentsInChildren<Platform>();
        foreach (Platform platform in platformArray)
        {
            platform.ResetState();
        }
    }

    public void ExplodePlatforms()
    {
        Platform[] platformArray = platforms.GetComponentsInChildren<Platform>();
        foreach (Platform platform in platformArray)
        {
            if (platform.GetComponent<Explode>())
            {
                platform.GetComponent<Explode>().Temporary(2);
            }
        }
    }
}
