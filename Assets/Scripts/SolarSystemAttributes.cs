using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemAttributes : MonoBehaviour
{
    public bool LoadAllSystems = false;
    public int numberOfExistingSolarSystems;
    public float starPadding = 0;
    public float columnSpacing = 1f;
    public float rowSpacing = .1f;
    public GameObject starTemplate;
    public Color[] starColors;
    [SerializeField]
    public Planet[] planets;
    // Start is called before the first frame update
    void Start()
    {
        //currently static in creation end scene
        if(LoadAllSystems)
        {
            LoadSystems(); 
        }

    }


    public void LoadSystems()
    {
        numberOfExistingSolarSystems = PlayerPrefs.GetInt("number of solar systems", 0);
        Debug.Log("SOLAR SYSTEMS: " + numberOfExistingSolarSystems);
        if (numberOfExistingSolarSystems > 0)
        {
            if(numberOfExistingSolarSystems > 100)
            {
                numberOfExistingSolarSystems = 100;
            }
            int objectsPerColumn = 8;
            int columns = Mathf.CeilToInt(numberOfExistingSolarSystems / objectsPerColumn);

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < objectsPerColumn; j++)
                {
                    {
                        int index = j * objectsPerColumn + i;
                        if(index >= numberOfExistingSolarSystems)
                        {
                            break;
                        }
                        GameObject star = LoadStar(i);

                        Vector3 coordinates = PlayerPrefsX.GetVector3("solar_system_" + index + "_star_coordinates");
                        float yPos = j * rowSpacing;
                        float zPos = i * columnSpacing;
                        coordinates.y = yPos;
                        coordinates.z = zPos;
                        star.transform.localPosition = coordinates;
                    }
                }

                //                LoadSystem(i, star.transform);

                //                    LoadSystem(i, star.transform);

            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject LoadStar(int index)
    {
        //every 5 stars, push z index 

        Vector2 info = PlayerPrefsX.GetVector2("solar_system_" + index + "_star_info", new Vector2(1,0));
        GameObject go = Instantiate(starTemplate, transform);
        go.transform.localScale = new Vector3(.1f, .1f, .1f);
        go.GetComponent<Renderer>().material.SetColor("_Color", starColors[(int)info.y]);
//        go.transform.localScale = new Vector3(info.x, info.x, info.x);
//        Debug.Log(go.transform.name + " NEW SCALE: " + go.transform.localScale);
        if(go.GetComponentInChildren<Core>())
        {
            go.GetComponentInChildren<Core>().SetScale();
        }
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
            Vector3 scaledPosition = solarSystemCoordinates[i];
//            scaledPosition = scaledPosition * .001f;
            planet.transform.localPosition = scaledPosition;
            float planetSize = solarSystemInfo[i].y;
            Vector3 planetScale = new Vector3(planetSize, planetSize, planetSize);
            planet.GetComponent<Orbit>().AssignStartingOrbitPosition();
            planet.transform.localScale = planetScale;

        }
    }
}
