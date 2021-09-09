using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewGalaxy : MonoBehaviour
{
//    public GameObject planet;
    public int galaxies;
    public float totalLightYearsTraveled;
    public int currentGalaxy = 0;
    public List<float> lightYearsBetweenGalaxies;
    public List<bool> vistedGalaxy;
    public int numberOfNearbyGalaxiesToShow = 5;
    public Vector3 coordinatesToMoveTo;
    public Camera cam;
    public Text galaxyDescription;
    public GameObject galaxyMarker;
    private string[] galaxyNames;
    private float[] galaxyLightyears;
    private List<bool> galaxyMarkerCreated;

    // Start is called before the first frame update
    void Start()
    {
        galaxyNames = PlayerPrefsX.GetStringArray("galaxy names");
        galaxyLightyears = PlayerPrefsX.GetFloatArray("galaxy lightyears");
        galaxyDescription.text = "";
        lightYearsBetweenGalaxies = new List<float>();
        vistedGalaxy = new List<bool>();
        galaxyMarkerCreated = new List<bool>();
        galaxies = galaxyNames.Length;
        Debug.Log(galaxies + " galaxies in universe");
        for (int i = 0; i < galaxies; i++)
        {
            Vector3[] g = PlayerPrefsX.GetVector3Array("galaxy " + i);
            vistedGalaxy.Add(false);
            galaxyMarkerCreated.Add(false);

        }
        lightYearsBetweenGalaxies.AddRange(galaxyLightyears);
        for(int i = 0; i < lightYearsBetweenGalaxies.Count; i++)
        {
            totalLightYearsTraveled += lightYearsBetweenGalaxies[i];
        }

/*        if (galaxies > 0)
        {
            SetupGalaxies();
        }
  */
        galaxyDescription.text = galaxyNames[0].ToString() + "\n(home)";


    }

    public void SetupGalaxies()
    {
        Debug.Log("Setting Up Galaxy!");
            currentGalaxy = 0;
            showGalaxy(currentGalaxy, false);
            populateNearbyGalaxies();  
    }

    private float calculateLightYearsAway(int currentGalaxy)
    {
        float lightYearsAway = 0;
        for (int i = 0; i < currentGalaxy; i++)
        {
            lightYearsAway += lightYearsBetweenGalaxies[i];
        }
        return lightYearsAway;
  
    }

    void populateNearbyGalaxies()
    {
        for (int i = currentGalaxy; i <= currentGalaxy + numberOfNearbyGalaxiesToShow; i++)
        { //i = 25; i < 25 < 25 + 5
            if(i < galaxies)
            {
                Debug.Log("SHOWING GALAXY# " + i);
                showGalaxy(i, false);
            }
        }
    }


    void showGalaxy(int galaxyNumber, bool moveThere)
    {
        Vector3[] coordinates = PlayerPrefsX.GetVector3Array("galaxy " + galaxyNumber);
            
        Debug.Log(coordinates.Length + " stars in galaxy #" + galaxyNumber);


            for (int i = 0; i < coordinates.Length; i++)
            {

                if (!vistedGalaxy[galaxyNumber])
                    {
                    GameObject star = ObjectPool.SharedInstance.GetPooledObject();
                    if(star != null)
                        {
                            star.transform.position = new Vector3(coordinates[i].x,
                                                                    coordinates[i].y,
                                                                    coordinates[i].z);
                            star.SetActive(true);
                        }
                    }
            }
    

        if (moveThere)
        {
            coordinatesToMoveTo = new Vector3(0, 0, calculateLightYearsAway(galaxyNumber) + galaxyLightyears[galaxyNumber]);
        }

        if (galaxyNumber < galaxies - 1)
        {
            vistedGalaxy[galaxyNumber] = true;
        }
}

    // Update is called once per frame

    public void NavigateForward()
    {
        Debug.Log("NAVIGATING FORWARD IN UNIVERSE!");
        //CHECK IF CURRENT GALAXY IS TE LAST / OR IS THIS A PLAY BUTTON TO CONTINUE JOURNEY?
        currentGalaxy++;

        //IF NOT THE LAST, CHECK IF ALREADY CREATED

        //IF NOT CREATED, CREAT WITH OFFSET
        if (currentGalaxy <= galaxies - 1)
        {
            showGalaxy(currentGalaxy - 1, true);
            if (!galaxyMarkerCreated[currentGalaxy])
            {
                GameObject gm = Instantiate(galaxyMarker);
                TextMesh[] tm = gm.GetComponentsInChildren<TextMesh>();

                if (tm.Length == 2)
                {
                    //found 2 text meshes, seems right
                    tm[0].text = galaxyNames[currentGalaxy].ToString() + " galaxy";
                    tm[1].text = calculateLightYearsAway(currentGalaxy).ToString() + " light-years from home";
                }

                Vector3 galaxyPosition = new Vector3(gm.transform.position.x, gm.transform.position.y, gm.transform.position.z);
                galaxyPosition.z = coordinatesToMoveTo.z;
                gm.transform.position = galaxyPosition;
                galaxyMarkerCreated[currentGalaxy] = true;


            }

        }
        else
        {
            currentGalaxy = galaxies - 1;
            showGalaxy(currentGalaxy, true);
            galaxyDescription.text = "Edge of your universe\n" + totalLightYearsTraveled + " light-years from home";
        }
        //DEACTIVATE GALAXIES BEHIND PLAYER

        ObjectPool.SharedInstance.DeactivateOffCameraObjects();
        if(currentGalaxy > 0)
        {
            vistedGalaxy[currentGalaxy - 1] = false;
        }
        populateNearbyGalaxies();
    }



    public void NavigateBackward()
    {
        currentGalaxy--;

        //CHECK IF ALREADY ON FIRST GALAXY

        //IF NOT TE FIRST, CHECK IF ALREADY CREATED

        //IF NOT CREATED, CREAT WITH OFFSET
        if (currentGalaxy > 0)
        {
            showGalaxy(currentGalaxy, true);
            galaxyDescription.text = galaxyNames[currentGalaxy].ToString() + " galaxy, " + calculateLightYearsAway(currentGalaxy + 1).ToString() + " years from home";

        }
        else
        {
            currentGalaxy = 0;
            galaxyDescription.text = galaxyNames[0].ToString() + " galaxy\n(home)";
            coordinatesToMoveTo = new Vector3(0, 1, -10);
            Debug.Log("Edge of the galaxy (back)");
        }

    }
    void Update()
    {
        if (cam) {

        }
        else
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, coordinatesToMoveTo, Time.deltaTime);
        }
    }
}
