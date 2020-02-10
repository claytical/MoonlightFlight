using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndlessLevel : MonoBehaviour {
	public GameObject LevelFailPanel;
    public GameObject[] breakableObjects;
    public Grid grid;
    public Text failureMessage;
    public bool inkEnabled;
    public int fliesReleased;
    public AudioClip success;
    public BallHolder ballHolder;
    private string selectedScene;
	private AsyncOperation AO;
	private GameState gameState;
	private int nextLevel;
	private string currentWorld;
    private bool levelFinished;
    private int totalRings;
    private int maxScore;
    private int setCount;
    private int powerUpIndex = 0;
    private bool hasPowerup = false;


    // Use this for initialization
    static System.Random rnd = new System.Random();

    void Start () {
//        Time.timeScale = 0f;
        fliesReleased = 0;
        setCount = 0;
        CreateRandomSetOfBreakables(grid.numberOfObjectsToPlace);
        gameState = (GameState)FindObjectOfType (typeof(GameState));
        //        PlaceRandomBreakable(Random.Range(1,3));
//        CreateRandomSetOfBreakables(Random.Range(4, 10));

        if (gameState != null) {
			currentWorld = gameState.SelectedWorld;
		}
        maxScore = gameObject.GetComponentsInChildren<Fly>().Length * 27;
    }

    void FixedUpdate () {
        if (!levelFinished)
        {
            ScanForCompletion();
        }
	}


    public void AddFliesReleased(int flies, float inkAmount, int multiplier)
    {
        fliesReleased += flies * multiplier;
    }
	public void ScanForCompletion() {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Disappearing");
        bool skipToFever = ballHolder.HasFullEnergy();
        if (gos.Length == 0 && setCount >= grid.sets ) //add check for current Set
        {
            //reset set counter
            setCount = 0;
            //old grid saved
            Grid oldGrid = grid;

            //EVENT #1 - FINISHED GRID
            //tell the current grid (open) to transition to finished grid
            if (!skipToFever)
            {
                Animator[] platforms = grid.platforms.GetComponentsInChildren<Animator>();
                for(int i = 0; i < platforms.Length; i++)
                {
                    platforms[i].SetTrigger("done");
                }
                grid.currentSet.BroadcastMessage("FinishedGrid", SendMessageOptions.DontRequireReceiver);
                ballHolder.AddSeed();
            }

            //set current grid to whatever grid is next (set 2)
            grid = grid.currentSet.nextGrid;
            //set ball holder's grid to our current grid 
            ballHolder.grid = grid;
            //turn off the old grid
            oldGrid.gameObject.SetActive(false);
            //set the grid of the ball to the current grid
            ballHolder.ship.GetComponent<Ball>().grid = grid;
            grid.gameObject.SetActive(true);
            CreateRandomSetOfBreakables(grid.numberOfObjectsToPlace);

            Debug.Log("Shutting down old grid: " + oldGrid.gameObject.name);


        }
        else if (gos.Length == 0) //place more breakables
        {
            //new set
            setCount++;
            CreateRandomSetOfBreakables(grid.numberOfObjectsToPlace);

        }

    }

/*
    public void EndOfLevel()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (gameState != null)
        {

            if (int.Parse(gameState.SelectedLevel) >= 10)
            {
                //unlock next world
                //WORLD COMPLETE UI
                gameState.SelectedWorld = nextWorldName;
                nextLevel = 1;
                PlayerPrefs.SetInt(gameState.SelectedWorld, 1);
                WorldCompletePanel.SetActive(true);
            }
            else
            {
                Debug.Log("Next Level Awaits...");
                //unlock next level
                nextLevel = int.Parse(gameState.SelectedLevel) + 1;
                PlayerPrefs.SetInt(gameState.SelectedWorld + "_" + nextLevel, 0);
                LevelCompletePanel.SetActive(true);
                if (LevelCompletePanel.GetComponent<LevelComplete>() != null)
                {
                    //max multiplier = 27
                    
                    LevelCompletePanel.GetComponent<LevelComplete>().SetScore(fliesReleased, maxScore);
                    Debug.Log("Setting score");
                }
            }
        }
        else
        {
            LevelCompletePanel.SetActive(true);
            if (LevelCompletePanel.GetComponent<LevelComplete>() != null)
            {
                LevelCompletePanel.GetComponent<LevelComplete>().SetScore(fliesReleased, maxScore);
                Debug.Log("Setting score");
            }

        }

    }
    */
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

/*        if (!hasPowerup)
        {
            Transform[] locations = grid.spawnLocations.GetComponentsInChildren<Transform>();
            GameObject obj = Instantiate(grid.powerUp, locations[powerUpIndex].position, Quaternion.identity, transform);
            hasPowerup = true;
            return obj;
        }
*/
        //        obj.SetActive(false);
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
