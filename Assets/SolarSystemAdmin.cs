using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemAdmin : MonoBehaviour
{

    public bool removeAllSolarSystems = false;
    public bool createStars;
    public int numberOfStars;
    public ParticleSystem solarFlares;
    public MeshRenderer core;

    public int[] planetAllocations;
    public int selectedStarColorIndex = 0;


    private int numberOfExistingSolarSystems;
    private List<Vector3> planetCoordinates;

    public Vector3 targetOffset = new Vector3(0, 5, 0);

    public SolarSystemAttributes systemAttributes;

// Start is called before the first frame update
void Start()
    {
        if (removeAllSolarSystems)
        {
            PlayerPrefs.DeleteAll();
        }

        numberOfExistingSolarSystems = PlayerPrefs.GetInt("number of solar systems", 0);
        ParticleSystem.MainModule main = solarFlares.main;
        main.startColor = systemAttributes.starColors[selectedStarColorIndex];
        core.material.color = systemAttributes.starColors[selectedStarColorIndex];
        if(createStars)
        {
            for(int i = 0; i < numberOfStars; i++)
            {
                Transform t = Create();
                Vector3 starPosition = t.position;
                starPosition.y = i*.1f;
                t.position = starPosition;
                Debug.Log("CREATE STAR #" + i + ": " + starPosition.y);

                systemAttributes.LoadSystem(i, t);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform Create()
    {
        solarFlares.GetComponent<SolarFlare>().SetCoreScale();

        //local scale should be 10

        float starSize = .1f;
        /*
         * STAR COLOR DETERMINED BY VEHICLE
         * 
         * 
         * TODO: Star is named after ship
         * Place this solar system within a galaxy: Stacked on an axis?
         * 
         * Stacked on Z Axis
         * All Player Mapped universe would show each "stacked" galaxy

         */

        PlayerPrefs.SetInt("First Solar System Created", 1);

        GameObject go = Instantiate(systemAttributes.starTemplate, transform);
        go.GetComponent<Renderer>().material.SetColor("_Color", systemAttributes.starColors[selectedStarColorIndex]);
        Vector3 starScale = new Vector3(starSize, starSize, starSize);
        Vector2 starInfo = new Vector2(starSize, selectedStarColorIndex);
        Vector3 starCoordinates = new Vector3(0, 0, 0);
        go.transform.localScale = new Vector3(.1f, .1f, .1f);

        PlayerPrefsX.SetVector3("solar_system_" + numberOfExistingSolarSystems + 1 + "_star_coordinates", starCoordinates);
        PlayerPrefsX.SetVector2("solar_system_" + numberOfExistingSolarSystems + 1 + "_star_info", starInfo);

        float distanceToStar = starScale.x;
        float padding = 1;

        planetCoordinates = new List<Vector3>();

        int numberOfPlanets = 0;

        //COUNT NUMBER OF PLANETS TO CREATE
        for (int i = 0; i < planetAllocations.Length; i++)
        {
            numberOfPlanets += planetAllocations[i];
        }

        //CREATE ARRAY FOR PLANETS TO STORE COORDINATES 
        Vector2[] planetInfo = new Vector2[numberOfPlanets];

        //ADD COORDINATES FOR EACH PLANET
        for (int i = 0; i < numberOfPlanets; i++)
        {
            Vector3 planetDistance = new Vector3(distanceToStar + padding, 0, 0);
            //planetDistance.y = Random.Range(-10, 10);
            planetCoordinates.Add(planetDistance);
            distanceToStar += padding + starScale.x;// + Random.Range(30,50);
        }
        //SHUFFLE UP ORDER COORDINATES FOR PLANET LIST
        planetCoordinates = ShuffleList(planetCoordinates);

        //ADD PLANET TYPE (WATER, GAS, ROCK) AND SIZE
        //PLANET ALLOCATIONS AND PLANET TEMPLATE ARRAYS SHOULD BE THE SAME SIZE
        //Allocations: 10,5,2
        //Number of Planets = 17

        //3
        int currentIndex = 0;

        for (int i = 0; i < planetAllocations.Length; i++)
        {
            // 0 = small
            // 1 = medium
            // 2 = large

            //ASSIGN TYPE AND SIZE FOR EACH ALLOCATION
            for (int j = 0; j < planetAllocations[i]; j++)
            {
                //size -- 0 * 2
                switch (i) {
                    case 0:
                        planetInfo[currentIndex + j].y = 1;
                        break;
                    case 1:
                        planetInfo[currentIndex + j].y = 1.5f;
                        break;
                    case 2:
                        planetInfo[currentIndex + j].y = 2f;
                        break;
                }

                //random planet type
                planetInfo[currentIndex + j].x = (int)Random.Range(0, systemAttributes.planets.Length);
            }
            currentIndex += planetAllocations[i];

        }


        PlayerPrefsX.SetVector2Array("solar_system_" + numberOfExistingSolarSystems + "_info", planetInfo);
        PlayerPrefsX.SetVector3Array("solar_system_" + numberOfExistingSolarSystems + "_coordinates", planetCoordinates.ToArray());
        PlayerPrefs.SetFloat("solar_system_" + numberOfExistingSolarSystems + "_size", distanceToStar);

        numberOfExistingSolarSystems++;
        PlayerPrefs.SetInt("number of solar systems", numberOfExistingSolarSystems);
        
        return go.transform;
/*
        Debug.Log("New Solar System Created: " + DialogueLua.GetVariable("Ship Name").asString);

        //DESTROY SHIP FROM FLEET
        DestroyShip(DialogueLua.GetVariable("Selected Ship").asInt);

        string s = PersistentDataManager.GetSaveData(); // Save state.
*/
    }

    private List<Vector3> ShuffleList(List<Vector3> ActualList)
    {

        List<Vector3> newList = ActualList;
        List<Vector3> outList = new List<Vector3>();

        int count = newList.Count;

        while (newList.Count > 0)
        {

            int rando = Random.Range(0, newList.Count);

            outList.Add(newList[rando]);
            newList.RemoveAt(rando);
        }

        return (outList);

    }


    public void DestroyShip(int shipId)
    {
        /*
              List<int> fleetTypes = PlayerPrefsX.GetIntArray("Fleet Ship Types").ToList();
              List<string> fleetNames = PlayerPrefsX.GetStringArray("Fleet Ship Names").ToList();
              string shipToDestroy = fleetNames[shipId];
              int destroyedShipType = fleetTypes[shipId];
              int totalShips = DialogueLua.GetVariable("Ships").asInt;

              if (totalShips > 0)
              {
                  fleetNames.RemoveAt(shipId);
                  fleetTypes.RemoveAt(shipId);
                  totalShips--;

                  //save current fleet
                  PlayerPrefsX.SetStringArray("Fleet Ship Names", fleetNames.ToArray());
                  PlayerPrefsX.SetIntArray("Fleet Ship Types", fleetTypes.ToArray());
                  DialogueLua.SetVariable("Ships", totalShips);

                  //add ship to graveyard
                  List<string> destroyedShipNames = PlayerPrefsX.GetStringArray("Destroyed Ship Names").ToList();
                  List<int> destroyedShipTypes = PlayerPrefsX.GetIntArray("Destroyed Ship Types").ToList();
                  List<int> destroyedShipEnergy = PlayerPrefsX.GetIntArray("Destroyed Ship Energy").ToList();

                  destroyedShipNames.Add(shipToDestroy);
                  destroyedShipTypes.Add(destroyedShipType);
                  destroyedShipEnergy.Add(energyAvailable);

                  PlayerPrefsX.SetStringArray("Destroyed Ship Names", destroyedShipNames.ToArray());
                  PlayerPrefsX.SetIntArray("Destroyed Ship Types", destroyedShipTypes.ToArray());
                  PlayerPrefsX.SetIntArray("Destroyed Ship Energy", destroyedShipEnergy.ToArray());
              }
              else
              {
                  Debug.Log("No ships available to destroy!");
              }
        */
    }


}
