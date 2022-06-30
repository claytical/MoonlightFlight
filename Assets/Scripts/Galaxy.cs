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
    public Text galaxyNameUI;
    public Text lightyearsUI;
    public Text numberOfPlanetsUI;
    public float timeSpentCreatingGalaxy = 0;
    public bool letsMakePlanets = false;
    private bool readyToLoadMain = false;
    private bool travelThroughGalaxy = false;
    private bool notEnoughEnergy = false;
    public GameObject backToMainButton;
    public GameObject newGalaxyPanel;
    public GameObject spawningArea;
    public AudioClip galaxyCreated;

    public InputField galaxyNameInput;

    public bool GenerateCleanGalaxy = false;
    public bool GenerateRandomGalaxy = false;
    private bool skipSavingGalaxy = false;
    private Release releaseObject;


    private string galaxyName;

    void Awake()
    {

    }

    public void SetRelease(Release r)
    {
        releaseObject = r;
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
/*        if(maxPlanets > 0)
        {
            galaxyName = newGalaxyName();
        }
 */
        calculateLightYearsAway();
        Vector3 newCameraPosition = Camera.main.transform.position;
        newCameraPosition.z += lightYearsAway;
        Camera.main.transform.position = newCameraPosition;
        galaxyName = newGalaxyName();
        Debug.Log("GAALXY: " + galaxyName);
    }

    private string newGalaxyName ()
    {
        string[] nm1 = new string[] { "Alpha", "Apus", "Aquila", "Ara", "Beta", "Canis", "Carina", "Comae", "Corona", "Crux", "Delta", "Draco", "Epsilon", "Gamma", "Lambda", "Lyra", "Nemo", "Omega", "Omicron", "Pavo", "Proxima", "Sagitta", "Serpens", "Sigma", "Theta", "Upsilon", "Ursa", "Vela", "Virgo", "Zeta", "", "", "", "", "", "" };
        string[] nm2 = new string[] { "Acallaris", "Achelois", "Adastreia", "Aegialeus", "Aegimius", "Alatheia", "Alcyoneus", "Aldebaran", "Amphiaraus", "Anadeia", "Andromeda", "Aquarii", "Arcturus", "Aristaeus", "Asteria", "Asteropaios", "Astraeus", "Aurigae", "Boreas", "Borysthenis", "Calesius", "Capella", "Cassiopeia", "Centauri", "Centaurus", "Chronos", "Cymopoleia", "Dioscuri", "Draconis", "Eioneus", "Eridani", "Eridanus", "Eubuleus", "Euphorion", "Eusebeia", "Euthenia", "Hemithea", "Hyperbius", "Hyperes", "Hyperion", "Icarius", "Ichnaea", "Ilioneus", "Kentaurus", "Leporis", "Librae", "Lyrae", "Majoris", "Miriandynus", "Myrmidon", "Nebula", "Nemesis", "Odysseus", "Ophiuchi", "Orion", "Orionis", "Orithyia", "Palioxis", "Peleus", "Perileos", "Perseus", "Phoroneus", "Polystratus", "Porphyrion", "Proioxis", "Sagittarius", "Sirius", "Solymus", "Zagreus", "Zephyrus" };
        string[] nm3 = new string[] { "Abyss", "Acorn", "Arrowhead", "Banana", "Beansprout", "Beanstalk", "Bell", "Blue Ribbon", "Blueberry", "Bottleneck", "Bowl", "Bull's Eye", "Bullet", "Butterfly", "Cat's Ear", "Cat's Eye", "Catterpillar", "Cherry", "Chickpea", "Clover", "Coconut", "Comet", "Crescent", "Crow's Feet", "Crown", "Dandelion", "Diamond", "Dragontooth", "Droplet", "Eagle Claw", "Eggshell", "Exploding", "Eyebrow", "Eyelash", "Falling", "Feather", "Fern Leaf", "Fingerprint", "Fisheye", "Fishscale", "Flame", "Football", "Grain", "Halo", "Heart", "Horseshoe", "Hurricane", "Icicle", "Iris", "Jellyfish", "Kettle", "Leaf", "Lemon", "Lightbulb", "Lilypad", "Lion's Mane", "Lion's Tail", "Maelstrom", "Meridian", "Mosaic", "Mouse", "Octopus", "Oculus", "Onion", "Owl Head", "Pear", "Pepper", "Pig's Tail", "Pinecone", "Ponytail", "Potato", "Red Ribbon", "Rippled", "Rose Petal", "Sawblade", "Seashell", "Serpent", "Serpent's Eye", "Sharkfin", "Sharktooth", "Shield", "Shooting Star", "Snail Shell", "Snowflake", "Soap Bubble", "Sparrow", "Spearpoint", "Spiderleg", "Spiderweb", "Spiral", "Starfish", "Strawberry", "Teacup", "Tiara", "Tiger Paw", "Tree Root", "Tree Trunk", "Turtle Shell", "Vortex", "Wave", "Whale's Tail", "Zodiac" };
        string[] nm4 = new string[] { "Nebula", "Galaxy", "Cloud", "Star System" };
        string[] nm5 = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "" };
        string[] nm6 = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "" };

        int randomPoint = Random.Range(0, 11);


            if (randomPoint < 3)
            {

                int rnd = (int) Mathf.Floor(Random.Range(0, nm1.Length));
                int rnd2 = (int) Mathf.Floor(Random.Range(0, nm2.Length));
                return nm1[rnd] + " " + nm2[rnd2];
            }
            else if (randomPoint < 5)
            {
                int rnd = (int)Mathf.Floor(Random.Range(0, nm2.Length));
                int rnd2 = (int)Mathf.Floor(Random.Range(0, nm4.Length));
                return nm2[rnd] + " " + nm4[rnd2];
            }
            else if (randomPoint < 8)
            {
            int rnd = (int)Mathf.Floor(Random.Range(0, nm3.Length));
            int rnd2 = (int)Mathf.Floor(Random.Range(0, nm4.Length));
                return nm3[rnd] + " " + nm4[rnd2];
            }
            else if (randomPoint < 9)
            {
                int rnd = (int)Mathf.Floor(Random.Range(0, nm5.Length));
                int rnd2 = (int)Mathf.Floor(Random.Range(0, nm5.Length));
                int rnd3 = (int)Mathf.Floor(Random.Range(0, nm6.Length));
                int rnd4 = (int)Mathf.Floor(Random.Range(0, nm6.Length));
                int rnd5 = (int)Mathf.Floor(Random.Range(0, nm6.Length));
                return nm5[rnd] + nm5[rnd2] + "-" + nm6[rnd3] + nm6[rnd4] + nm6[rnd5];
            }
            else
            {
            int rnd = (int)Mathf.Floor(Random.Range(0, nm5.Length));
            int rnd2 = (int)Mathf.Floor(Random.Range(0, nm5.Length));
            int rnd3 = (int)Mathf.Floor(Random.Range(0, nm5.Length));
            int rnd4 = (int)Mathf.Floor(Random.Range(0, nm6.Length));
            int rnd5 = (int)Mathf.Floor(Random.Range(0, nm6.Length));
            return nm5[rnd] + nm5[rnd2] + nm5[rnd3] + " " + nm6[rnd4] + nm6[rnd5] + nm5[rnd5];
            }
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

    public void SetGalaxyName()
    {
        galaxyName = galaxyNameInput.text;
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

                //notify release script
                releaseObject.StopCheckingControls();
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
            if (Time.frameCount % 5 == 0)
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
                    letsMakePlanets = false;
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
            galaxyList.Add(galaxyNameUI.text);
            PlayerPrefsX.SetStringArray("galaxy names", galaxyList.ToArray());
            Debug.Log("Saving Galaxy Name:" + galaxyNameUI.text);
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
        spawningArea.SetActive(false);
        galaxyNameInput.text = galaxyName;
//        galaxyNameUI.text = galaxyName;
        lightyearsUI.text = lightYearsTraveled.ToString("0 Light Years Away");
        numberOfPlanetsUI.text = maxPlanets.ToString("0 Planets"); 
        ui.text = "A new galaxy has been born.";
        newGalaxyPanel.SetActive(true);
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
