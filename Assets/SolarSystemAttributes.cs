using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemAttributes : MonoBehaviour
{
    public bool LoadAllSystems = false;
    public bool removeAllSolarSystems = false;
    public int numberOfExistingSolarSystems;

    public GameObject starTemplate;
    public Color[] starColors;
    [SerializeField]
    public Planet[] planets;
    // Start is called before the first frame update
    void Start()
    {
        //currently static in creation end scene
        if(removeAllSolarSystems)
        {
            PlayerPrefs.DeleteAll();
        }
        if(LoadAllSystems)
        {
 
            numberOfExistingSolarSystems = PlayerPrefs.GetInt("number of solar systems", 0);
            if(numberOfExistingSolarSystems > 0)
            {
                Debug.Log("Loading " + numberOfExistingSolarSystems + " Solar Systems...");
                for(int i = 0; i < numberOfExistingSolarSystems; i++)
                {
                    GameObject star = LoadStar(i);
//                    LoadSystem(i, star.transform);
                }
            }
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject LoadStar(int index)
    {
        Vector3 coordinates = PlayerPrefsX.GetVector3("solar_system_" + index + "_star_coordinates");
        if(index > 0)
        {
            float distanceBetweenSystems = PlayerPrefs.GetFloat("solar_system_" + (index - 1) + "_size", 0) + PlayerPrefs.GetFloat("solar_system_" + index + "_size", 0);
            coordinates.x += distanceBetweenSystems *.01f;
        }
        Vector3 info = PlayerPrefsX.GetVector3("solar_system_" + index + "_star_info");
        GameObject go = Instantiate(starTemplate, transform);
        go.transform.localPosition = coordinates;
        go.GetComponent<Renderer>().material.SetColor("_Color", starColors[(int)info.y]);
        Vector3 starScale = new Vector3(.01f, .01f, .01f);
        go.transform.localScale = starScale;
        return go;
    }

    public void LoadSystem(int index, Transform parent)
    {
        Vector2[] solarSystemInfo = PlayerPrefsX.GetVector2Array("solar_system_" + index + "_info");
        Vector3[] solarSystemCoordinates = PlayerPrefsX.GetVector3Array("solar_system_" + index + "_coordinates");

        for (int i = 0; i < solarSystemCoordinates.Length; i++)
        {

            GameObject planet = Instantiate(planets[(int)solarSystemInfo[i].x].template, parent);
            Debug.Log("PLANET COORDS: " + solarSystemCoordinates[i]);
            planet.transform.localPosition = solarSystemCoordinates[i];
            float planetSize = solarSystemInfo[i].y;
            Vector3 planetScale = new Vector3(planetSize, planetSize, planetSize);
            planet.GetComponent<Orbit>().AssignStartingOrbitPosition();
            planet.transform.localScale = planetScale;

        }
    }
}
