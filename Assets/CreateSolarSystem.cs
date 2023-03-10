using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum PlanetType
{
    Rock = 0,
    Gas = 1,
    Water = 2
};
[System.Serializable]

public struct PlanetRequirement
{
    public int energyRequired;
    public int diameter; //SMALL, MEDIUM, LARGE
    public NumberButton button;
}

[System.Serializable]

public struct Planet
{
    public GameObject template;
    public PlanetType type;
}


public class CreateSolarSystem : MonoBehaviour
{
    private GameState gameState;
    public Color[] starColors;
    public GameObject starTemplate;
    public PlanetRequirement[] planetRequirements;
    
//    public Planet[] planets;

    public Text energyToExpend;

    public ParticleSystem solarFlares;
    public MeshRenderer core;
    private int[] planetAllocations;
   

    public int energyAvailable;
    private int energySpent = 0;
    private int selectedStarColorIndex = 0;
    private int numberOfExistingSolarSystems;

    private List<Vector3> planetCoordinates;

    public Transform newStar;
    public float transitionDuration = 2.0f;
    public Vector3 targetOffset = new Vector3(0, 5, 0);

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float transitionStartTime;

    public bool admiringNewSystem = false;

    public SolarSystemAttributes systemAttributes;

    // Start is called before the first frame update
    void Start()
    {
        energyAvailable = 400;
//        energyAvailable = PlayerPrefs.GetInt("energy collected", 100);
        

        SetEnergyText();

        planetAllocations = new int[planetRequirements.Length];

        for(int i = 0; i < planetRequirements.Length; i++)
        {
            planetRequirements[i].button.SetCost(planetRequirements[i].energyRequired);
        }

        numberOfExistingSolarSystems = PlayerPrefs.GetInt("number of solar systems", 0);
        if (gameState != null)
        {
            VehicleType vehicle = gameState.GetVehicle();
            if (((int)vehicle) < starColors.Length)
            {
                //color match exists
                selectedStarColorIndex = ((int)vehicle);
            }
            else
            {
                //default galaxy creation
            }
        }
        else
        {
            //No game state present, default galaxy creation values

        }
        ParticleSystem.MainModule main = solarFlares.main;
        main.startColor = starColors[selectedStarColorIndex];
        core.material.color = starColors[selectedStarColorIndex];


        Debug.Log("Number of systems: " + numberOfExistingSolarSystems);
        Debug.Log("Star Color Index: " + selectedStarColorIndex);
    }

    private void Update()
    {
        if(admiringNewSystem)
        {
            float transitionProgress = (Time.time - transitionStartTime) / transitionDuration;
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition, transitionProgress);
        }

    }

    public void SetEnergyText()
    {
        energyToExpend.text = EnergyAvailableToSpend().ToString("0");
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
    public void ShowLastCreatedSystem()
    {
        systemAttributes.LoadSystem(numberOfExistingSolarSystems-1, transform);
    }

    public void Create(int starSize)
    {
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



        for(int i = 0; i < planetRequirements.Length; i++)
        {
            planetAllocations[i] = planetRequirements[i].button.currentAmount;
        }

        GameObject go = Instantiate(starTemplate, transform);
        go.GetComponent<Renderer>().material.SetColor("_Color", starColors[selectedStarColorIndex]);
        Vector3 starScale = new Vector3(starSize, starSize, starSize);
        go.transform.localScale = starScale;
        PlayerPrefsX.SetVector3("solar_system_" + numberOfExistingSolarSystems + 1 + "_star_coordinates", new Vector3(0, 0, 0));
        PlayerPrefsX.SetVector3("solar_system_" + numberOfExistingSolarSystems + 1 + "_star_info", new Vector2(starSize, selectedStarColorIndex));

        float distanceToStar = starScale.x;
        float padding = 5;
        planetCoordinates = new List<Vector3>();
        int numberOfPlanets = 0;
        //COUNT NUMBER OF PLANETS TO CREATE
        for(int i = 0; i < planetAllocations.Length; i++)
        {
            numberOfPlanets += planetAllocations[i];
        }

        //CREATE ARRAY FOR PLANETS TO STORE COORDINATES 
        Vector2[] planetInfo = new Vector2[numberOfPlanets];

        //ADD COORDINATES FOR EACH PLANET
        for (int i = 0; i < numberOfPlanets; i++)
        {
            Vector3 planetDistance = new Vector3(distanceToStar + padding, 0, 0);
            planetDistance.y = Random.Range(-10, 10);
            planetCoordinates.Add(planetDistance);
            distanceToStar += padding + starScale.x + Random.Range(30,50);
            Debug.Log(i + ": PLANET DISTANCE: " + planetDistance + " DISTANCE TO STAR: " + distanceToStar);
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
            //ASSIGN TYPE AND SIZE FOR EACH ALLOCATION
            for(int j = 0; j < planetAllocations[i]; j++)
            {
                //size
                planetInfo[currentIndex + j].y = planetRequirements[i].diameter;
                Debug.Log("PLANET DIAMETER " + (currentIndex + j) + ":" + planetRequirements[i].diameter);
                //random planet type
                planetInfo[currentIndex + j].x = (int)Random.Range(0, systemAttributes.planets.Length);
            }
            Debug.Log("CURRENT INDEX: " + currentIndex);
            currentIndex += planetAllocations[i];

        }


        PlayerPrefsX.SetVector2Array("solar_system_" + numberOfExistingSolarSystems + "_info", planetInfo);
        PlayerPrefsX.SetVector3Array("solar_system_" + numberOfExistingSolarSystems + "_coordinates", planetCoordinates.ToArray());
        PlayerPrefs.SetFloat("solar_system_" + numberOfExistingSolarSystems + "_size", distanceToStar);

        numberOfExistingSolarSystems++;
        PlayerPrefs.SetInt("number of solar systems", numberOfExistingSolarSystems);

    }


    public void Admire()
    {
        newStar = GetComponentInChildren<ParticleSystem>().gameObject.transform;
        Camera.main.transform.Rotate(new Vector3(30, 0, 0));
        startPosition = transform.position;
        targetPosition = newStar.position + targetOffset;
        transitionStartTime = Time.time;
        admiringNewSystem = true;
    }

    public int EnergyAvailableToSpend()
    {
        int cost = 0;
        for(int i = 0; i < planetRequirements.Length; i++)
        {
            cost += planetRequirements[i].button.currentAmount * planetRequirements[i].energyRequired;
        }
        return energyAvailable - cost;
    }

}
