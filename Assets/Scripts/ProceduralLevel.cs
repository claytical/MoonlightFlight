using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ProceduralLevel : MonoBehaviour {
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

    private Vector3 originalPosition;
    private SetInfo[] sets;
    private string selectedScene;
	private AsyncOperation AO;
    private int setCount;

    // Use this for initialization
    static System.Random rnd = new System.Random();

    void Start () {
        sets = patterns.GetComponentsInChildren<SetInfo>();
        //EACH GRID PLAYS HAS SETS OF BREAKABLES
        setCount = 1;
    }

    public void Play()
    {
        set.gameObject.SetActive(true);
        //CREATE BREAKABLES IN GRID
        CreateRandomSetOfBreakables(set);

    }

    public void BuildNextSet()
    {
        music.ChangeTrack();

        SetInfo previousSet = set;
        set = set.currentSet.SetNextSet();
        setCount = 1;

        //start new music, set procedural set's current next grid active -> NEW GRID SHOWN
        previousSet.currentSet.FinishedSet();

        //link ship and grid scripts
//        vehicle.LinkSet(set);

        //populate breakables for current grid
        CreateRandomSetOfBreakables(set);
        previousSet.MovePlatformsOffScreen();
    }

    public void RemovePlatforms()
    {
        Debug.Log("Removing Platforms");
        int numberOfPlatforms = set.platforms.GetComponentsInChildren<Platform>().Length;
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            if (set.platforms.GetComponentsInChildren<Platform>()[i])
            {
                if (set.platforms.GetComponentsInChildren<Platform>()[i].platform.GetComponent<Explode>())
                    set.platforms.GetComponentsInChildren<Platform>()[i].platform.GetComponent<Explode>().Temporary(2);
            }
            }
    }


    public bool AllObjectsCollected()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Collect");
        Debug.Log("GAME OBJECTS ON SCREEN: " + gos.Length);
        Debug.Log("SET COUNT: " + setCount);
        Debug.Log("NUMBER OF SETS: " + set.sets);

        if (gos.Length == 0 && setCount >= set.sets)
        {
            Debug.Log("Finished Pattern, Moving to Next Set");
            return true;
        }
        else if(gos.Length == 0)
        {
            setCount++;
            CreateRandomSetOfBreakables(set);
        }
            return false;
    }

    public void CreateRandomSetOfBreakables(SetInfo s)
    {

        Debug.Log("Calling Energy Creation");
        if (s.spawnEverything)
        {
            s.SetAutoSpawnLocations();
            Debug.Log("Creating " + s.spawnLocations.Length + " breakables.");
            for (int i = 0; i < s.spawnLocations.Length; i++)
            {
                GameObject obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], set.spawnLocations[i].position, Quaternion.identity, transform);
                
            }
        }
        else
        {
            int numberOfBreakablesToPlace = Random.Range(s.spawnLocations.Length/2, s.spawnLocations.Length);
            int[] series = Reservoir(numberOfBreakablesToPlace, s.spawnLocations.Length);
            for (int i = 0; i < series.Length; i++)
            {
                GameObject obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], set.spawnLocations[i].position, Quaternion.identity, transform);

            }
        }
    }

    //Resevoir Sampling
    //https://visualstudiomagazine.com/articles/2013/07/01/generating-distinct-random-array-indices.aspx

    static int[] Reservoir(int n, int range)
    {
        int[] result = new int[n];
        for (int i = 0; i < n; ++i)
            result[i] = i;

        for (int t = n; t < range; ++t)
        {
            int m = rnd.Next(0, t + 1);
            if (m < n) result[m] = t;
        }
        return result;
    }


	IEnumerator loadScene() {
		AO = SceneManager.LoadSceneAsync (selectedScene, LoadSceneMode.Single);
		AO.allowSceneActivation = false;
		while (AO.progress < 0.9f) {
			yield return null;
		}
		AO.allowSceneActivation = true;
	}


}
