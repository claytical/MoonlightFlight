using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class Galaxy : MonoBehaviour
{
    public int maxPlanets = 10;
    public float lightYearsTraveled = 0;
    public float lightYearsAway = 0;
    public int currentPlanet = 0;
    public GameObject planet;
    public List<Vector3> coordinates;
    public Text ui;
    public float timeSpentCreatingGalaxy = 0;
    public bool letsMakePlanets = false;
    private bool readyToLoadMain = false;
    private bool loadingNewScene = false;
    private bool createdGalaxy = false;
    private bool travelThroughGalaxy = false;
    private bool notEnoughEnergy = false;
    public GameObject backToMainButton;
    public AudioClip galaxyCreated;

    public bool GenerateCleanGalaxy = false;
    public bool GenerateRandomGalaxy = false;
    private bool skipSavingGalaxy = false;


    JSONObject galaxyNames;
    public TextAsset json;
    private string galaxyName;

    void Awake()
    {

    }

    void Start()
    {
        int timesPlayed = PlayerPrefs.GetInt("times played", 0);
        timesPlayed++;
        PlayerPrefs.SetInt("times played", timesPlayed);

        if(GenerateRandomGalaxy)
        {
            if(GenerateCleanGalaxy)
            {
                PlayerPrefs.DeleteAll();

            }
            lightYearsTraveled = Random.Range(10, 120);
            maxPlanets = Random.Range(10, 500);
        }
        else
        {
            lightYearsTraveled = PlayerPrefs.GetFloat("light years traveled", 0);
            maxPlanets = PlayerPrefs.GetInt("planets collected", 0);

        }
        coordinates = new List<Vector3>();
        if(maxPlanets > 0)
        {
            galaxyNames = new JSONObject(json.ToString());
            JSONObject galaxyList = galaxyNames["names"][0];
            string chars = "AB 23456789";
            galaxyName = galaxyNames.list[0][Random.Range(0, galaxyNames.list[0].Count)].str + " " + chars[Random.Range(0, chars.Length)];
        }
        calculateLightYearsAway();
        Vector3 newCameraPosition = Camera.main.transform.position;
        newCameraPosition.z += lightYearsAway;
        Camera.main.transform.position = newCameraPosition;

    }

    private void calculateLightYearsAway()
    {
//        int currentGalaxy = PlayerPrefs.GetInt("galaxies", 0);
        float[] galaxyLightyears = PlayerPrefsX.GetFloatArray("galaxy lightyears");

        for (int i = 0; i < galaxyLightyears.Length; i++)
        {
            lightYearsAway += galaxyLightyears[i];
            //            Vector3[] g = PlayerPrefsX.GetVector3Array("galaxy " + i);
            //            lightYearsAway += g.Length;
        }
        Debug.Log("Current System is " + lightYearsAway + " light-years away");
    }

    void Update()
    {
        if (letsMakePlanets)
        {
            if (maxPlanets > 0)
            {
                CreatingSystem();

            }
            else
            {
                Debug.Log("Not enough energy to create a planet...");
                NotEnoughEnergyToCreatePlanet();
                backToMainButton.SetActive(true);
                notEnoughEnergy = true;
                letsMakePlanets = false;
            }
        }

        if (travelThroughGalaxy)
        {
            TravelThroughGalaxy();
        }

        if (readyToLoadMain)
        {
            // GetComponent<SceneControl>().SetSceneToLoad("Main");
            // GetComponent<SceneControl>().SelectScene();
            backToMainButton.SetActive(true);

            readyToLoadMain = false;
        }

        if(notEnoughEnergy)
        {
            //waiting for input
        }

    }

    // Update is called once per frame
    private void CreatingSystem()
    {
            if (Time.frameCount % 25 == 0)
            {

                if (currentPlanet < maxPlanets)
                {
                    CreatePlanet();
                    GetComponent<AudioSource>().PlayOneShot(galaxyCreated);
                    currentPlanet++;
                }
                else
                {
                    ShowGalaxyName();
                    createdGalaxy = true;
                    letsMakePlanets = false;
                    Debug.Log("Saving Galaxy....");
                    //RUNNING TWICE!
//                    SaveGalaxyAndLoadMain();
                    travelThroughGalaxy = true;
                }
            }
    }

    private void TravelThroughGalaxy()
    {
        Vector3 newCameraPosition = Camera.main.transform.position;
        newCameraPosition.z = Mathf.Lerp(newCameraPosition.z, maxPlanets, Time.deltaTime * .2f);
        Camera.main.transform.position = newCameraPosition;

        if (newCameraPosition.z >= lightYearsTraveled - 1)
        {
            travelThroughGalaxy = false;
        }

    }
    public void SaveGalaxyAndLoadMain()
    {
        if(!skipSavingGalaxy)
        {
            int galaxyCount = PlayerPrefs.GetInt("galaxies", 0);
            Debug.Log(galaxyCount + " galaxies in existence.");
            Debug.Log("Saving galaxy #" + galaxyCount);
            PlayerPrefsX.SetVector3Array("galaxy " + (galaxyCount).ToString(), coordinates.ToArray());
            Debug.Log("Saving Galaxy Coordinates");

            float[] galaxyLYArray = PlayerPrefsX.GetFloatArray("galaxy lightyears");
            List<float> galaxyLightyears = new List<float>();
            galaxyLightyears.AddRange(galaxyLYArray);
            galaxyLightyears.Add(lightYearsTraveled);
            PlayerPrefsX.SetFloatArray("galaxy lightyears", galaxyLightyears.ToArray());
            Debug.Log("Saving Galaxy Lightyears...");

            string[] galaxyArray = PlayerPrefsX.GetStringArray("galaxy names");
            List<string> galaxyList = new List<string>();
            galaxyList.AddRange(galaxyArray);
            Debug.Log("GALAXY RANGE: " + galaxyArray.ToString());
            galaxyList.Add(galaxyName);
            Debug.Log("GALAXY ADDED RANGE: " + galaxyList);
            PlayerPrefsX.SetStringArray("galaxy names", galaxyList.ToArray());
            Debug.Log("Saving Galaxy Name:" + galaxyName);
            galaxyCount++;
            PlayerPrefs.SetInt("galaxies", galaxyCount);

            Debug.Log("Setting galaxy count to " + galaxyCount + " and ready to load new scene");

        }
        else
        {
            Debug.Log("No galaxy to save...");
        }
    }

    private void ShowGalaxyName()
    {
        ui.text = galaxyName + " Galaxy has been born.\nIt spans " + lightYearsTraveled + " light-years and has " + maxPlanets + " stars!";
        ui.enabled = true;
        readyToLoadMain = true;

    }

    private void NotEnoughEnergyToCreatePlanet()
    {
        ui.text = "Not enough energy collected to create a new galaxy...";
        ui.enabled = true;
        skipSavingGalaxy = true;
    }

    private void CreatePlanet()
    {
        GameObject p = (GameObject)Instantiate(planet, transform);
        Vector3 position = new Vector3(Random.Range(-200, 200),
                                               Random.Range(-200, 200),
                                               Random.Range(lightYearsAway, lightYearsAway + lightYearsTraveled));
        p.transform.position = position;
        //planet size
        coordinates.Add(position);
    }


}
