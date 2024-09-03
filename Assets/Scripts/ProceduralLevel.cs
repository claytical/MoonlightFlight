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
    public GameObject portalPrefab;
    public GameObject newPlane;
    public int setsBeforePortal = 3;
    public float initialSpawnWaitTime = 1f;
    public float waitTimeIncrement = 0.5f;
    public float inactivityTime = 10f;  // Time in seconds to check for inactivity
    public GameObject specialItemPrefab;  // Prefab of the special item to spawn
    public CanvasMeter breakablesInSet;
    public CanvasMeter breakablesCollected;
    public ParticleSystem breakableMeterFX;
    private SetInfo[] sets;
    private int setCount;
    private bool allBreakablesSpawned;
    private float currentSpawnWaitTime;
    private int completedSets = 0;
    private int loops = 1;
    void Start()
    {
        setCount = 1;
        allBreakablesSpawned = false;
        currentSpawnWaitTime = initialSpawnWaitTime;

        // Start the level
        Play();

        // Start the inactivity check coroutine
        StartCoroutine(CheckForVehicleInactivity());
    }

    public void SetBreakables(int amount)
    {
        breakablesCollected.gameObject.SetActive(true);
        breakablesInSet.gameObject.SetActive(true);
        breakablesInSet.Populate(amount);
        breakablesCollected.Populate(0);
    }

    public void SetBreakablesCollected(int amount)
    {

            breakablesCollected.Populate(amount);
    }

    private void AttachAllPlatforms()
    {
        Platform[] platforms = Resources.FindObjectsOfTypeAll<Platform>();
        for(int i = 0; i < platforms.Length; i++)
        {
            platforms[i].AttachLevel(this);
        }
    }
    private IEnumerator HandlePortalAppearance()
    {
        if (portalPrefab != null)
        {
            GameObject portal = Instantiate(portalPrefab, Vector3.zero, Quaternion.identity);
            portal.transform.position = new Vector3(0, 0, 0);

            Debug.Log("Portal appeared.");

            // Optionally wait a frame to ensure initialization
            yield return null;

            // Ensure the portal still exists before continuing
            if (portal != null && !portal.Equals(null))
            {
                yield return new WaitForSeconds(3f);

                // Safe to destroy the portal
//                Destroy(portal);
//                yield return null;  // Ensure destruction is processed before continuing

                BuildNextSet();
                Debug.Log("Portal clear, continuing to the next set.");
            }
            else
            {
                Debug.LogWarning("Portal was destroyed before further processing.");
            }
        }
        else
        {
            Debug.LogWarning("Portal prefab is not assigned.");
        }
    }
    public void Play()
    {
        if (set == null)
        {
            Debug.LogError("Set is not assigned in ProceduralLevel.");
            return;
        }
        AttachAllPlatforms();
        set.gameObject.SetActive(true);
        StartCoroutine(WaitForSetInitializationAndStartSpawning());
    }

    private IEnumerator WaitForSetInitializationAndStartSpawning()
    {
        while (!set.IsInitialized())
        {
            yield return null;
        }

        StartCoroutine(SpawnBreakables());
    }

    private IEnumerator SpawnBreakables()
    {
        while (!allBreakablesSpawned)
        {
            Debug.Log("Not all breakables have spawned.");
            set.SpawnNextBatchOfBreakables();

            if (set.AllBreakablesSpawned())
            {
                allBreakablesSpawned = true;
                Debug.Log("All breakables have spawned.");

            }

            yield return new WaitForSeconds(currentSpawnWaitTime);
        }
    }
    public void NextPlane(Transform transform)
    {
        GameObject go = Instantiate(newPlane);
        go.transform.position = transform.position;
    }
    public bool AllObjectsCollected()
    {
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collect");
        return collectables.Length == 0;
    }

    public void RemovePlatforms()
    {
        set.ExplodePlatforms();
    }

    public void BuildNextSet()
    {
            ProceduralInfo proceduralInfo = set.GetComponent<ProceduralInfo>();
        //MAKING MIDI CHANGES    
        //music.ChangeTrack();

            SetInfo previousSet = set;
            set = proceduralInfo.SetNextSet();
            if (set == null)
            {
                Debug.LogWarning("No next set was selected.");
                return;
            }
            setCount++;

            //previousSet.MoveOffScreen(Vector3.zero, 0.5f);
            StartCoroutine(WaitForPreviousSetToFinish(previousSet));

            currentSpawnWaitTime += waitTimeIncrement;
    }
    public bool AllBreakablesCollected()
    {
        // Check if there are any breakables left in the scene
        GameObject[] remainingBreakables = GameObject.FindGameObjectsWithTag("Collect");
        return remainingBreakables.Length == 0;
    }
    private IEnumerator WaitForPreviousSetToFinish(SetInfo previousSet)
    {
        // Continuously check every 2 seconds if all breakables have been collected
        while (!AllBreakablesCollected())
        {
            yield return new WaitForSeconds(2f); // Wait for 2 seconds before checking again
        }

        // Once all breakables are collected, proceed with the transition
        allBreakablesSpawned = false;  // Reset the flag to prepare for the next set
        set.ResetPlatforms();          // Reset platforms in the next set
        previousSet.gameObject.SetActive(false);
        completedSets++;
        if (completedSets % setsBeforePortal == 0)
        {
            yield return StartCoroutine(HandlePortalAppearance());
        }
        else
        {
            Debug.Log("Waiting for next portal: " + completedSets + " sets, " + setsBeforePortal + " sets before portal appearance");
        }


        Play();                        // Start the next set
    }
    private IEnumerator CheckForVehicleInactivity()
    {
        while (true)
        {
            yield return new WaitForSeconds(inactivityTime);

            Vehicle[] vehicles = FindObjectsOfType<Vehicle>();
            Vector3[] lastVehiclePositions = new Vector3[vehicles.Length];

            bool allVehiclesInactive = true;

            for (int i = 0; i < vehicles.Length; i++)
            {
                if (Vector3.Distance(vehicles[i].transform.position, lastVehiclePositions[i]) > 0.01f)
                {
                    allVehiclesInactive = false;
                    break;
                }
                lastVehiclePositions[i] = vehicles[i].transform.position; // Update last known positions
            }

            if (allVehiclesInactive)
            {
                SpawnSpecialItem();
            }
        }
    }

    private void SpawnSpecialItem()
    {
        if (specialItemPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(0, 5, 0); // Example position, adjust as needed
            Instantiate(specialItemPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Special item spawned due to vehicle inactivity.");
        }
        else
        {
            Debug.LogWarning("Special item prefab is not assigned.");
        }
    }
}
