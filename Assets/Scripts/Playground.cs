using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

[System.Serializable] public struct Loot
{
    public GameObject item;
    public int weight;
}


public class Playground : MonoBehaviour {

//    public Loot[] availableLoot;
    public Transform lastEnergyCollectionPosition;
//    public Vector3 lootDropLocation;
//    public SceneControl scene;

    public GameObject patterns;
    public ParkingLot lot;
    public ProceduralMusic music;
    public GameObject warpMenu;
    public GameObject outpostLocation;
    public GameObject dialogueActor;
    public Vector3 dialogueActorSecondPosition;
    public AudioSource effectsAudio;

    public SetInfo set;
    public bool useAnimationForPlatforms = false;
    public float waitTimeForGameOver;
    public RectTransform offLimitTouchPoint;
    public RectTransform dialogueCanvas;

    private Vector3 originalPosition;
    private SetInfo[] sets;
    private bool warping = false;
    private bool warpingBack = false;
    private string selectedScene;
	private AsyncOperation AO;
	private GameState gameState;
    private int setCount;
    private Vehicle vehicle;
    private bool buildingNewSet = false;
    private float gameOverTime = 9999;
    private Vector3 dialogueActorOriginalPosition;
    // Use this for initialization
    static System.Random rnd = new System.Random();

    void Start () {
        dialogueActorOriginalPosition = dialogueActor.transform.position;
        VehicleType vehicleId = (VehicleType)DialogueLua.GetVariable("Vehicle Type").AsInt;
        vehicle = lot.SelectVehicle(vehicleId);
        vehicle.offLimitTouchPoint = offLimitTouchPoint;
//        lootDropLocation = Vector3.zero;      
        sets = patterns.GetComponentsInChildren<SetInfo>();

        //EACH GRID PLAYS HAS SETS OF BREAKABLES
        setCount = 1;

        //ASSIGN NEXT SETS

        set.gameObject.SetActive(true);

        //PASS GRID INFO TO SHIP
        vehicle.LinkSet(set);

        vehicle.SetPlayground(this);

        //CREATE BREAKABLES IN GRID
        CreateRandomSetOfBreakables(set.numberOfObjectsToPlace);

    }
    public void Warp()
    {
        dialogueActor.transform.position = dialogueActorSecondPosition;
        vehicle.offLimitTouchPoint = dialogueCanvas;
        foreach (Transform child in dialogueCanvas)
        {
            child.gameObject.SetActive(false);
        }
        lot.vehicle.GetComponentInChildren<Vehicle>().Stasis(true);
        originalPosition = Camera.main.transform.position;
        warping = true;
        warpingBack = false;
    }

    public void WarpBack()
    {
        dialogueActor.transform.position = dialogueActorOriginalPosition;

        foreach (Transform child in dialogueCanvas)
        {
            child.gameObject.SetActive(true);
        }

        vehicle.offLimitTouchPoint = offLimitTouchPoint;
        warping = false;
        warpingBack = true;
        lot.vehicle.GetComponentInChildren<Vehicle>().Stasis(false);
    }


    void Update () {
        if (warping)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, outpostLocation.transform.position, Time.deltaTime);
          
        }

        if(warpingBack)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, originalPosition, Time.deltaTime);
        }

        ScanForCompletion();
            if(useAnimationForPlatforms)
            {
            Debug.Log("Using Animation For Platforms...");
                if (buildingNewSet)
                {
                    if (HavePlatformsDisappeared())
                    {
                    Debug.Log("Platforms have disappeared!");
                        BuildNextSet();
                    }
                }

            }

        if (gameOverTime <= Time.time && gameOverTime != 9999)
        {
            lot.SetEnergy();
            DialogueLua.SetVariable("Energy Available", lot.energyCollected);
            DialogueLua.SetVariable("Nukes", 0);
            DialogueLua.SetVariable("Brakes", 0);
            DialogueManager.PlaySequence("LoadLevel(End)");
            gameOverTime = 9999;
        }



    }

    private void ResetPlatforms()
    {
        for (int i = 0; i < set.platforms.GetComponentsInChildren<Platform>().Length; i++)
        {
            set.platforms.GetComponentsInChildren<Platform>()[i].finished = true;
        }

    }
    public void BuildNextSet()
    {
        music.ChangeTrack();
        if (useAnimationForPlatforms)
        {
            ResetPlatforms();
        }
        else
        {
            //MOVE PLATFORMS OFF SCREEN
        }

        SetInfo previousSet = set;
        //set current grid to whatever grid is next (set 2) -> NEXT GRID SELECTED
        set = set.currentSet.SetNextSet();
        //BROKE ALL RINGS, CLEARED ALL SETS, RETURN BACK TO FIRST SET OF NEXT SET OF SETS 
        setCount = 1;

        //start new music, set procedural set's current next grid active -> NEW GRID SHOWN
        previousSet.currentSet.FinishedSet();

        //link ship and grid scripts
        vehicle.LinkSet(set);

        //populate breakables for current grid
        CreateRandomSetOfBreakables(set.numberOfObjectsToPlace);
        //turn off old grid - NO GRIDS SHOWN
        //Does this screw with the audio sources?
        //CUE MOVE OFF SCREEN

        previousSet.MovePlatformsOffScreen();

        buildingNewSet = false;
    }


    public void DropNuke()
    {
        Hazard[] hazards = set.platforms.GetComponentsInChildren<Hazard>();
        for(int i = 0; i < hazards.Length; i++)
        {
            //should 10 second delay be controlled through powerup?
            hazards[i].gameObject.GetComponent<Explode>().Temporary(10);
        }
    }

    public bool HavePlatformsDisappeared()
    {
        int numberOfPlatforms = set.platforms.GetComponentsInChildren<Platform>().Length;
        int numberOfFinishedPlatforms = 0;
        for (int i = 0; i < numberOfPlatforms; i++) {
            if(set.platforms.GetComponentsInChildren<Platform>()[i].finished) {
                set.platforms.GetComponentsInChildren<Platform>()[i].TurnOffCollision();
                numberOfFinishedPlatforms++;
            }
        }
        if(numberOfPlatforms == numberOfFinishedPlatforms)
        {
            return true;
        }
        return false;
    }

    public void ScanForCompletion() {
        if (vehicle) {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Disappearing");
            if (useAnimationForPlatforms)
            {
                if (gos.Length == 0 && setCount >= set.sets && !buildingNewSet)
                {
                    //EVENT #1 - FINISHED GRID
                    //tell the current grid (open) to transition to finished grid

                    Animator[] platforms = set.platforms.GetComponentsInChildren<Animator>();

                    buildingNewSet = true;
                }
                else if (gos == null && setCount < set.sets) //place more breakables
                {
                    //new set
                    setCount++;
                    CreateRandomSetOfBreakables(set.numberOfObjectsToPlace);

                }
            }
            else
            {
                if(set)
                {
                    if (gos.Length == 0 && setCount >= set.sets)
                    {
                        Debug.Log("CALLING BUILD NEXT SET!");
                        BuildNextSet();
                    }
                    else if (gos.Length == 0 && setCount < set.sets)
                    {
                        Debug.Log("MORE SETS!");
                        setCount++;
                        CreateRandomSetOfBreakables(set.numberOfObjectsToPlace);

                    }

                }
                else
                {
                    Debug.Log("No Set!");
                }

            }

        }
    }

    

    public void GameOver()
    {
        gameOverTime = waitTimeForGameOver + Time.time;

    }

    public void Wait()
    {
        set.currentSet.Waiting();
    }

    public void PlaceRandomBreakable(int amount)
    {

        Transform[] locations = set.spawnLocations.GetComponentsInChildren<Transform>();
        GameObject obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], locations[Random.Range(0, locations.Length)].position, Quaternion.identity, transform);
        
        //Place new object
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        Vector3 origin = Camera.main.ViewportToScreenPoint(new Vector3(-7, 0, 0));
        float startX = Camera.main.transform.position.x - (Screen.width / 2);
        float objX = obj.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        float objY = obj.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        //+ (_prefabSize.x / 2) + (_prefabSize.x * x);
        float startY = Camera.main.transform.position.y - (Screen.height / 2);
        //+ (_prefabSize.y / 2) + (_prefabSize.y * y);

        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(objX, Screen.width - objX), Random.Range(objY, Screen.height - objY), Camera.main.farClipPlane / 2));
  //      Vector3 position = new Vector3(origin.x, origin.y, 0);
        obj.transform.position = screenPosition;
//        GameObject obj = Instantiate(breakableObjects[Random.Range(0, breakableObjects.Length)], screenPosition, Quaternion.identity, transform);
        
    }

    public void CreateRandomSetOfBreakables(int n)
    {
        Transform[] locations = set.spawnLocations.GetComponentsInChildren<Transform>();
        Debug.Log(n + " SPAWNED FROM: " + set.spawnLocations.gameObject.name);
        int[] series = Reservoir(n, locations.Length);

        if (n <= 1)
        {
            GameObject obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], locations[series[0]].position, Quaternion.identity, transform);
        }
        else
        {
            for (int i = 0; i < n; i++) {
                if (locations[series[i]].position != Vector3.zero) {
                    GameObject obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], locations[series[i]].position, Quaternion.identity, transform);
                }
            }
        }
    }

    public void CreateRandomSetOfBreakablesWithSwitches(int n)
    {
        Transform[] locations = set.spawnLocations.GetComponentsInChildren<Transform>();
        //sets need to be in triplets
        int[] series = Reservoir(n, locations.Length);
        for (int i = 0; i < series.Length - 2; i+=3)
        {
            //switch
            GameObject switch_obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], locations[series[i]].position, Quaternion.identity, transform);
            //lock
            GameObject lock_obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], locations[series[i+1]].position, Quaternion.identity, transform);
            //lock
            GameObject obj_treasure = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], locations[series[i + 2]].position, Quaternion.identity, transform);

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


    public void SetSceneToLoad(string s) {
        if(s.Contains("Main"))
        {
            Destroy(gameState.gameObject);
        }
        selectedScene = s;
	}

	public void SelectScene() {
		StartCoroutine("loadScene");
	}

	IEnumerator loadScene() {
		AO = SceneManager.LoadSceneAsync (selectedScene, LoadSceneMode.Single);
		AO.allowSceneActivation = false;
		while (AO.progress < 0.9f) {
			yield return null;
		}
		AO.allowSceneActivation = true;
	}

    public void RetryLevel()
    {
        if (gameState)
        {
            selectedScene = "Remix";
            SelectScene();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


}
