using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable] public struct Loot
{
    public GameObject item;
    public int weight;
}


public class ProceduralLevel : MonoBehaviour {

    public Color shipColor;
    public Color[] gradients;
    public Color borderColor;
    public Color itemColor;
    public Color boxColor;
    public Color secondaryColor;
    public Color hazardColor;
    public Color energyColor;

    public Loot[] availableLoot;
    public Vector3 lastEnergyCollectionPosition;
    public Vector3 lootDropLocation;
    public SceneControl scene;

	public GameObject LevelFailPanel;
    private SetInfo[] sets;
    public GameObject patterns;
    public Text failureMessage;
    public ParkingLot lot;
    public ProceduralMusic music;
    public GameObject warpMenu;
    public GameObject outpostLocation;
    private Vector3 originalPosition;
    private bool warping = false;
    private bool warpingBack = false;


    public GameObject ProgressPanel;

    public AudioSource effectsAudio;
    private string selectedScene;
	private AsyncOperation AO;
	private GameState gameState;

    private int setCount;
    private int powerUpIndex = 0;
    private int setIndex;
    private Vehicle vehicle;
    public SetInfo set;
    private bool buildingNewSet = false;
    public bool useAnimationForPlatforms = false;
    public float waitTimeForGameOver;
    private float gameOverTime = 9999;
    public RectTransform offLimitTouchPoint; 

    // Use this for initialization
    static System.Random rnd = new System.Random();

    void Start () {
        //SETUP SHIP NAME AND CREATE IT
        gameState = (GameState)FindObjectOfType(typeof(GameState));
        if (gameState != null)
        {
            vehicle = lot.SelectVehicle(gameState.GetVehicle());
            //using .2f for nerfing forces to balance overdoing it. using .1f to balance bounciness.
            vehicle.force += PlayerPrefs.GetInt(gameState.GetVehicle().ToString() + "_" + "MaxForce", 0) * .2f;
            vehicle.maxForce += PlayerPrefs.GetInt(gameState.GetVehicle().ToString() + "_" + "Force", 0) * .2f;
            if (vehicle.GetComponent<Rigidbody2D>())
            {
//                vehicle.GetComponent<Rigidbody2D>().sharedMaterial.bounciness = PlayerPrefs.GetInt(gameState.GetVehicle().ToString() + "_" + "Bounce", 0) * .1f;

            }

        }
        else
        {
            vehicle = lot.DefaultVehicle();
        }
        vehicle.offLimitTouchPoint = offLimitTouchPoint;
        lootDropLocation = Vector3.zero;      
        Debug.Log("Nerfing Vehicle Force");


        sets = patterns.GetComponentsInChildren<SetInfo>();

        //EACH GRID REPEATS A GIVEN NUMBER, RESET TO ZERO
        setCount = 0;
        
        //PICK A STARTING GRID
        setIndex = Random.Range(0, sets.Length);

        //ASSIGN THAT GRID TO START
        set = sets[setIndex];

        //ASSIGN NEXT SETS

//        set.currentSet.SetNextSet();
        
        //PASS GRID INFO TO SHIP
        vehicle.LinkSet(set);

        //SET TRACK FOR THE SHIP
        vehicle.SetTrack(this);


        //CREATE BREAKABLES IN GRID
        CreateRandomSetOfBreakables(sets[setIndex].numberOfObjectsToPlace);
//        set.GetComponent<ProceduralInfo>().start.TransitionTo(0);

    }
    public void Warp()
    {
        lot.vehicle.GetComponentInChildren<Vehicle>().Stasis(true);
        originalPosition = Camera.main.transform.position;
        warping = true;
        warpingBack = false;
    }

    public void WarpBack()
    {
        warping = false;
        warpingBack = true;
        lot.vehicle.GetComponentInChildren<Vehicle>().Stasis(false);
    }
    public void LootDrop()
    {
        lot.vehicle.GetComponentInChildren<Vehicle>().lootAvailable = true;


        int total = 0;
        int[] lootRange = new int[availableLoot.Length];


        for(int i = 0; i < availableLoot.Length; i++)
        {
            total += availableLoot[i].weight;
            lootRange[i] = total;
        }

        int roll = Random.Range(0, total);
        int selectedLoot = -1;

        for(int i = 1; i <= lootRange.Length; i++)
        {
            if(roll > lootRange[i -1] && roll < lootRange[i])
            {
                selectedLoot = i;
            }
        }
        
        if(selectedLoot == -1)
        {
            selectedLoot = 0;
        }

        

        /*Transform[] locations = set.spawnLocations.GetComponentsInChildren<Transform>();
        if (!locations[powerUpIndex].GetComponent<PowerUp>())
        {
        */
            GameObject obj = Instantiate(availableLoot[selectedLoot].item, lastEnergyCollectionPosition, Quaternion.identity, transform);
//            GameObject energyTransfer = Instantiate(vehicle.energyTransfer, vehicle.transform);
//            energyTransfer.GetComponent<EnergyTransfer>().startingPoint = vehicle.transform;
//            energyTransfer.GetComponent<EnergyTransfer>().endingPoint = obj.transform;
            //hard destroy when location can't lerp
//            Destroy(energyTransfer, 2f);
            lootDropLocation = obj.transform.position;
        /*    
    }

        else
        {
            Debug.Log("No loot dropped, location already had loot...");
        }
    */
    }

    public void MaxEnergyReached()
    {
        LootDrop();
    }

    void Update () {
        Debug.Log("Building Next Set: " + buildingNewSet);
        if (warping)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, outpostLocation.transform.position, Time.deltaTime);
            Debug.Log("Warping to outpost...");
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
            PlayerPrefs.SetFloat("light years traveled", lot.lightYearsTraveled);
            PlayerPrefs.SetInt("planets collected", lot.planetsCollected);
            scene.SetSceneToLoad("End");
            scene.SelectScene();
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
        lot.LightYearTraveled();
        if (useAnimationForPlatforms)
        {
            ResetPlatforms();
        }
        else
        {
            //MOVE PLATFORMS OFF SCREEN
        }

        ProgressPanel.SetActive(true);
        ProgressPanel.GetComponent<ProgressPanel>().ResetTimer();
        ProgressPanel.GetComponentInChildren<Text>().text = lot.lightYearsTraveled.ToString("0") + " Light-years traveled\n" + lot.planetsCollected + " units of energy collected";

        SetInfo previousSet = set;
        //set current grid to whatever grid is next (set 2) -> NEXT GRID SELECTED
        set = set.currentSet.SetNextSet();
        //BROKE ALL RINGS, CLEARED ALL SETS                    
        setCount = 0;

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
//        fullEnergyLoops.LimitLoopsPlaying(grid);


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

                    for (int i = 0; i < platforms.Length; i++)
                    {
//                        platforms[i].SetTrigger("done");
                    }
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
                if (gos.Length == 0 && setCount >= set.sets)
                {
                    Debug.Log("CALLING BUILD NEXT SET!");
                    BuildNextSet();
                }
                else if(gos.Length == 0 && setCount < set.sets)
                {
                    Debug.Log("MORE SETS!");
                    setCount++;
                    CreateRandomSetOfBreakables(set.numberOfObjectsToPlace);

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

            for (int i = 0; i < n; i++)
                {
                if (locations[series[i]].position != Vector3.zero)
                    {
                        if(lot.vehicle.GetComponentInChildren<Vehicle>().lootAvailable && locations[series[i]].position == lootDropLocation)
                        {
                            Debug.Log("Loot in spawn space... don't spawn here again until its collected");
                        }
                        else
                        {
                            if (i == series.Length - 1)
                            {

                            //using a series, set position for recall later
                                if (series.Length > 0)
                                {
                                    powerUpIndex = i;
                                }

                                GameObject obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], locations[series[i]].position, Quaternion.identity, transform);

                            }
                        else
                            {
                                GameObject obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], locations[series[i]].position, Quaternion.identity, transform);
                            }
                        }

                } else
                {
                    Debug.Log("position was vector zero");
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
