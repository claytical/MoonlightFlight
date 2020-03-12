using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndlessLevel : MonoBehaviour {
	public GameObject LevelFailPanel;
    public GameObject shipUnlockedPanel;
    public GameObject[] breakableObjects;
    public PowerUpMusic fullEnergyLoops;
    public LevelGrid[] grids;
    public Text failureMessage;
    public bool inkEnabled;
    public AudioClip success;
    public Dock dock;

    private string selectedScene;
	private AsyncOperation AO;
	private GameState gameState;
	private int nextLevel;
	private string currentWorld;
    private int totalRings;
    private int maxScore;
    private int setCount;
    private int powerUpIndex = 0;
    private int gridIndex;
    private Ship ship;
    private LevelGrid grid;
    private bool buildingNewGrid = false;
    public bool useAnimationForPlatforms = false;


    // Use this for initialization
    static System.Random rnd = new System.Random();

    void Start () {
        //SETUP SHIP NAME AND CREATE IT
        gameState = (GameState)FindObjectOfType(typeof(GameState));
        ship = dock.SelectShip(gameState.GetShip());

        //EACH GRID REPEATS A GIVEN NUMBER, RESET TO ZERO
        setCount = 0;
        
        //PICK A STARTING GRID
        gridIndex = Random.Range(0, grids.Length);
        
        //ASSIGN THAT GRID TO START
        grid = grids[gridIndex];

        //PASS GRID INFO TO SHIP
        ship.LinkGrid(grid);

        //SET LEVEL FOR THE SHIP
        ship.SetLevel(this);

        
        //CREATE BREAKABLES IN GRID
        CreateRandomSetOfBreakables(grids[gridIndex].numberOfObjectsToPlace);

    }
    
    public void MaxEnergyReached()
    {

        dock.AddSeeds(1);
        CreatePowerUp();
        grid.PowerUp();
        fullEnergyLoops.LimitLoopsPlaying(grid);

    }

    void Update () {
            ScanForCompletion();
            if(useAnimationForPlatforms)
            {
                if (buildingNewGrid)
                {
                    if (HavePlatformsDisappeared())
                    {
                        BuildNextGrid();
                    }
                }

            }
            else
            {

            }
    }

    private void ResetPlatforms()
    {
        for (int i = 0; i < grid.platforms.GetComponentsInChildren<Platform>().Length; i++)
        {
            grid.platforms.GetComponentsInChildren<Platform>()[i].finished = true;
        }

    }
    public void BuildNextGrid()
    {
        if (useAnimationForPlatforms)
        {
            ResetPlatforms();
        }

        LevelGrid previousGrid = grid;
        //set current grid to whatever grid is next (set 2) -> NEXT GRID SELECTED
        grid = grid.currentSet.SetNextGrid();
        //BROKE ALL RINGS, CLEARED ALL SETS                    
        setCount = 0;

        //start new music, set procedural set's current next grid active -> NEW GRID SHOWN
        previousGrid.currentSet.FinishedGrid();
        //reward player
        dock.AddSeeds(1);

        //link ship and grid scripts
        ship.LinkGrid(grid);

        //populate breakables for current grid
        Debug.Log("CREATING " + grid.numberOfObjectsToPlace + " FOR " + grid.name);
        CreateRandomSetOfBreakables(grid.numberOfObjectsToPlace);
        //turn off old grid - NO GRIDS SHOWN
        previousGrid.gameObject.SetActive(false);

        buildingNewGrid = false;
        fullEnergyLoops.LimitLoopsPlaying(grid);


    }

    public bool HavePlatformsDisappeared()
    {
        int numberOfPlatforms = grid.platforms.GetComponentsInChildren<Platform>().Length;
        int numberOfFinishedPlatforms = 0;
        for (int i = 0; i < numberOfPlatforms; i++) {
            if(grid.platforms.GetComponentsInChildren<Platform>()[i].finished) {
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
        if (ship) {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Disappearing");
            if (useAnimationForPlatforms)
            {
                if (gos.Length == 0 && setCount >= grid.sets && !buildingNewGrid)
                {
                    //EVENT #1 - FINISHED GRID
                    //tell the current grid (open) to transition to finished grid

                    Animator[] platforms = grid.platforms.GetComponentsInChildren<Animator>();

                    for (int i = 0; i < platforms.Length; i++)
                    {
                        platforms[i].SetTrigger("done");
                    }
                    buildingNewGrid = true;
                }
                else if (gos.Length == 0 && setCount < grid.sets) //place more breakables
                {
                    //new set
                    setCount++;
                    CreateRandomSetOfBreakables(grid.numberOfObjectsToPlace);

                }
            }
            else
            {
                if (gos.Length == 0 && setCount >= grid.sets)
                {
                    BuildNextGrid();
                }
                else if(gos.Length == 0 && setCount < grid.sets)
                {
                    setCount++;
                    CreateRandomSetOfBreakables(grid.numberOfObjectsToPlace);

                }

            }

        }
    }

    
    private bool unlockedNewShips()
    {
        //TODO: instantiate new unlocked item screen
        for(int i = 0; i < dock.ships.Length; i++)
        {
            if (dock.ships[i].GetComponentInChildren<Ship>())
            {
                Debug.Log("Has Ship Script");
                if (PlayerPrefs.HasKey(dock.ships[i].GetComponentInChildren<Ship>().type.ToString()))
                {
                    //ship was locked before playing
                    int seedsRequired = PlayerPrefs.GetInt(dock.ships[i].GetComponentInChildren<Ship>().type.ToString());
                    Debug.Log("SEEDS REQUIRED: " + seedsRequired);
                    if (seedsRequired <= PlayerPrefs.GetInt("seeds"))
                    {
                        Debug.Log("UNLOCKED " + dock.ships[i].GetComponentInChildren<Ship>().type.ToString());
                        //UNLOCKED NEW SHIP!
                        shipUnlockedPanel.SetActive(true);
                        shipUnlockedPanel.GetComponent<ShipUnlockPanel>().UnlockShip(dock.ships[i].GetComponentInChildren<Ship>().type);
                        return true;
                    }

                }
            }

        }

        return false;
    }

    public void GameOver()
    {
        dock.SetSeeds();
        //        finished = true;
        if(!unlockedNewShips())
        {
            LevelFailPanel.SetActive(true);

        }
        grid.currentSet.Waiting();
        if (dock.seedsCollected > 0)
        {
            failureMessage.text = "You collected " + dock.seedsCollected + " seeds of light on your journey.";
        }
        else
        {
            failureMessage.text = "You didn't collect any seeds of light on your journey.";

        }
    }

    public void Wait()
    {
        grid.currentSet.Waiting();
    }

    public void PlaceRandomBreakable(int amount)
    {

        Transform[] locations = grid.spawnLocations.GetComponentsInChildren<Transform>();
        GameObject obj = Instantiate(grid.breakables[Random.Range(0, grid.breakables.Length)], locations[Random.Range(0, locations.Length)].position, Quaternion.identity, transform);
        
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
        Transform[] locations = grid.spawnLocations.GetComponentsInChildren<Transform>();
        int[] set = Reservoir(n, locations.Length);
        Debug.Log("SET: " + set.ToString());
        for (int i = 0; i < set.Length; i++)
        {
            if (locations[set[i]].position != Vector3.zero)
            {
                if (i == set.Length - 1)
                {

                    //set position for recall later
                    powerUpIndex = i;
                } else
                {
                    GameObject obj = Instantiate(grid.breakables[Random.Range(0, grid.breakables.Length)], locations[set[i]].position, Quaternion.identity, transform);

                }
            }
        }
    }

    public GameObject CreatePowerUp()
    {
        Debug.Log("Creating Powerup");
        grid.PowerUp();
        Transform[] locations = grid.spawnLocations.GetComponentsInChildren<Transform>();
        if (!locations[powerUpIndex].GetComponent<PowerUp>()) {
            GameObject obj = Instantiate(grid.powerUp, locations[powerUpIndex].position, Quaternion.identity, transform);
            return obj;
        }

        return null;
    }
    public void CreateRandomSetOfBreakablesWithSwitches(int n)
    {
        Transform[] locations = grid.spawnLocations.GetComponentsInChildren<Transform>();
        //sets need to be in triplets
        int[] set = Reservoir(n, locations.Length);
        for (int i = 0; i < set.Length - 2; i+=3)
        {
            //switch
            GameObject switch_obj = Instantiate(grid.breakables[Random.Range(0, grid.breakables.Length)], locations[set[i]].position, Quaternion.identity, transform);
            //lock
            GameObject lock_obj = Instantiate(grid.breakables[Random.Range(0, grid.breakables.Length)], locations[set[i+1]].position, Quaternion.identity, transform);
            //lock
            GameObject obj_treasure = Instantiate(grid.breakables[Random.Range(0, grid.breakables.Length)], locations[set[i + 2]].position, Quaternion.identity, transform);

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

    /*
        public void NextLevel() {
            if (gameState != null) {
                gameState.SelectedLevel = nextLevel.ToString ();
                selectedScene = gameState.SelectedWorld + "_" + nextLevel;
                SelectScene ();
            }
        }

        public void RetryLevel() {
            if (gameState) {
                selectedScene = currentWorld + "_" + gameState.SelectedLevel;
                SelectScene ();
            } else {
                SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
            }
        }
        */
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
            selectedScene = "Procedural";
            SelectScene();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


}
