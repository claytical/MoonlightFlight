using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralLevel : MonoBehaviour
{
    public RemixManager remix;
    public Loot[] availableLoot;
    public GameObject LevelFailPanel;
    public GameObject patterns;
    public Text failureMessage;
    public ParkingLot lot;
    public ProceduralMusic music;
    public GameObject ProgressPanel;
    public AudioSource effectsAudio;
    public SetInfo set;

    private SetInfo[] sets;
    private int setCount;
    private bool allBreakablesSpawned = false;

    void Start()
    {
//        sets = patterns.GetComponentsInChildren<SetInfo>();
        setCount = 1;
        allBreakablesSpawned = false;

        // Directly call Play to start the level
        Play();
    }

    public void Play()
    {
        if (set == null)
        {
            Debug.LogError("Set is not assigned in ProceduralLevel.");
            return;
        }

        set.gameObject.SetActive(true);

        // Check if SetInfo is initialized
        StartCoroutine(WaitForSetInitializationAndStartSpawning());
    }
    private IEnumerator WaitForSetInitializationAndStartSpawning()
    {
        // Wait until the SetInfo component is fully initialized
        while (!set.IsInitialized())
        {
            yield return null;
        }

        Debug.Log("Set active and initialized, starting to spawn breakables.");
        StartCoroutine(SpawnBreakables());
    }
    private IEnumerator SpawnBreakables()
    {
        Debug.Log("Starting SpawnBreakables coroutine...");
        while (!allBreakablesSpawned)
        {
            Debug.Log("Spawning next batch of breakables...");
            set.SpawnNextBatchOfBreakables();

            if (set.AllBreakablesSpawned())
            {
                allBreakablesSpawned = true;
                Debug.Log("All breakables spawned.");
            }

            yield return new WaitForSeconds(1f);  // Wait a second before spawning the next batch
        }
    }

    public bool AllObjectsCollected()
    {
        if (!allBreakablesSpawned)
        {
            return false;
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Collect");
        return gos.Length == 0;
    }

    public void RemovePlatforms()
    {
        set.ExplodePlatforms();  // Trigger the explosion effect and temporary deactivation
    }

    public void BuildNextSet()
    {
        if (AllObjectsCollected())
        {
            // Access ProceduralInfo from the current set
            ProceduralInfo proceduralInfo = set.GetComponent<ProceduralInfo>();
            music.ChangeTrack();
            SetInfo previousSet = set;
            set = proceduralInfo.SetNextSet(); // PickWeightedSet();  // Pick the next set based on weight
            if (set == null)
            {
                Debug.LogWarning("No next set was selected.");
                return;
            }
            setCount++;

            previousSet.MoveOffScreen(Vector3.zero, 0.5f);  // Move previous platforms off screen
            StartCoroutine(WaitForPreviousSetToFinish(previousSet));
        }
    }

    private SetInfo PickWeightedSet()
    {
        int totalWeight = 0;

        // Calculate the total weight of all sets
        foreach (var s in sets)
        {
            totalWeight += s.weight;
        }

        int randomWeight = Random.Range(0, totalWeight);

        // Select a set based on the weighted random value
        foreach (var s in sets)
        {
            if (randomWeight < s.weight)
            {
                return s;
            }
            randomWeight -= s.weight;
        }

        // Fallback in case of any error, though this should never happen
        return sets[0];
    }

    private IEnumerator WaitForPreviousSetToFinish(SetInfo previousSet)
    {
        yield return new WaitForSeconds(2f);  // Wait for the previous platforms to be moved off screen

        allBreakablesSpawned = false;
        set.ResetPlatforms();  // Reset platforms in the next set
        Play();  // Start the next set
    }
}
