﻿using System.Collections;
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
    public Color primaryColor;
    public Color secondaryColor;
    public Color hazardColor;
    public Color energyColor;

    public Loot[] availableLoot;

    public SceneControl scene;

	public GameObject LevelFailPanel;
    public GameObject vehicleUnlockedPanel;
    private SetInfo[] sets;
    public GameObject patterns;
    public Text failureMessage;
    public ParkingLot lot;
    public ProceduralMusic music;

    public GameObject ProgressPanel;

    public AudioSource effectsAudio;
    private string selectedScene;
	private AsyncOperation AO;
	private GameState gameState;

    private int setCount;
    private int powerUpIndex = 0;
    private int setIndex;
    private Vehicle vehicle;
    private SetInfo set;
    private bool buildingNewSet = false;
    public bool useAnimationForPlatforms = false;


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
        Debug.Log("Creating Random Breakables for " + sets[setIndex].name);
        CreateRandomSetOfBreakables(sets[setIndex].numberOfObjectsToPlace);
//        set.GetComponent<ProceduralInfo>().start.TransitionTo(0);

    }

    void LootDrop()
    {

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

        Transform[] locations = set.spawnLocations.GetComponentsInChildren<Transform>();
        if (!locations[powerUpIndex].GetComponent<PowerUp>())
        {
            //TODO: Change to Loot Drop with weights

            GameObject obj = Instantiate(availableLoot[selectedLoot].item, locations[powerUpIndex].position, Quaternion.identity, transform);
        }

        else
        {
            Debug.Log("No loot dropped, location already had loot...");
        }
    }

    public void MaxEnergyReached()
    {
        LootDrop();
    }

    void Update () {
            ScanForCompletion();
            if(useAnimationForPlatforms)
            {
                if (buildingNewSet)
                {
                    if (HavePlatformsDisappeared())
                    {
                        BuildNextSet();
                    }
                }

            }
            else
            {
                
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
        Debug.Log("CREATING " + set.numberOfObjectsToPlace + " FOR " + set.name);
        CreateRandomSetOfBreakables(set.numberOfObjectsToPlace);
        //turn off old grid - NO GRIDS SHOWN
        //Does this screw with the audio sources?
        //CUE MOVE OFF SCREEN

        previousSet.MovePlatformsOffScreen();

        buildingNewSet = false;
//        fullEnergyLoops.LimitLoopsPlaying(grid);


    }

    public bool HavePlatformsDisappeared()
    {
        int numberOfPlatforms = set.platforms.GetComponentsInChildren<Platform>().Length;
        int numberOfFinishedPlatforms = 0;
        for (int i = 0; i < numberOfPlatforms; i++) {
            if(set.platforms.GetComponentsInChildren<Platform>()[i].finished) {
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
                        platforms[i].SetTrigger("done");
                    }
                    buildingNewSet = true;
                }
                else if (gos.Length == 0 && setCount < set.sets) //place more breakables
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
                    BuildNextSet();
                }
                else if(gos.Length == 0 && setCount < set.sets)
                {
                    setCount++;
                    CreateRandomSetOfBreakables(set.numberOfObjectsToPlace);

                }

            }

        }
    }

    
    private bool unlockedNewVehicles()
    {
        //TODO: instantiate new unlocked item screen
        for(int i = 0; i < lot.vehicles.Length; i++)
        {
            if (lot.vehicles[i].GetComponentInChildren<Vehicle>())
            {
                Debug.Log("Has Ship Script");
                if (PlayerPrefs.HasKey(lot.vehicles[i].GetComponentInChildren<Vehicle>().type.ToString()))
                {
                    //ship was locked before playing
                    int energyRequired = PlayerPrefs.GetInt(lot.vehicles[i].GetComponentInChildren<Vehicle>().type.ToString());
                    Debug.Log("energy REQUIRED: " + energyRequired);
                    if (energyRequired <= PlayerPrefs.GetInt("energy"))
                    {
                        Debug.Log("UNLOCKED " + lot.vehicles[i].GetComponentInChildren<Vehicle>().type.ToString());
                        //UNLOCKED NEW SHIP!
                        vehicleUnlockedPanel.SetActive(true);
                        vehicleUnlockedPanel.GetComponent<VehicleUnlockPanel>().UnlockVehicle(lot.vehicles[i].GetComponentInChildren<Vehicle>().type);
                            
                           //lot.vehicles[i].GetComponentInChildren<Vehicle>().type);
                        return true;
                    }

                }
            }

        }

        return false;
    }

    public void GameOver()
    {
        lot.SetEnergy();
        //        finished = true;

        PlayerPrefs.SetFloat ("light years traveled", lot.lightYearsTraveled);
        PlayerPrefs.SetInt("planets collected", lot.planetsCollected);
        scene.SetSceneToLoad("End");
        scene.SelectScene();
        
//        LevelFailPanel.SetActive(true);
//        Time.timeScale = 0f;
//        set.currentSet.Waiting();
 /*
        if (lot.energyCollected > 0)
        {
            failureMessage.text = "You collected " + lot.energyCollected + " light on your journey.";
        }
        else
        {
            failureMessage.text = "You didn't collect any light on your journey.";

        }
   */
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
            Debug.Log("N <= 1 OBJECT:" + locations[series[0]].name);
        }
        else
        {

            for (int i = 0; i < n; i++)
                {
                Debug.Log("I: " + i);
                if (locations[series[i]].position != Vector3.zero)
                    {
                    if (i == series.Length - 1)
                    {

                        //using a series, set position for recall later
                            if(series.Length > 0)
                        {
                            powerUpIndex = i;
                        }

                        GameObject obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], locations[series[i]].position, Quaternion.identity, transform);
                        Debug.Log("ELSE I == SERIES " + i + "OBJECT:" + locations[series[i]].name);

                    }
                    else
                    {
                        GameObject obj = Instantiate(set.breakables[Random.Range(0, set.breakables.Length)], locations[series[i]].position, Quaternion.identity, transform);
                        Debug.Log("ELSE " + i + "OBJECT:" + locations[series[i]].name);

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
