using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System.Linq;

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

    public int energyAvailable;
    public ParticleSystem solarFlares;
    public MeshRenderer core;



    public Color[] starColors;
    public GameObject starTemplate;
    public Transform newStar;
    public int selectedStarColorIndex = 0;

    public PlanetRequirement[] planetRequirements;

    public string firstConversation;
    public string conversation;

    public Text energyToExpend;

    private int[] planetAllocations;
    private int numberOfExistingSolarSystems;
    private List<Vector3> planetCoordinates;

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
        numberOfExistingSolarSystems = PlayerPrefs.GetInt("number of solar systems", 0);

        energyAvailable = DialogueLua.GetVariable("Energy Available").asInt;

        SetEnergyText();

        planetAllocations = new int[planetRequirements.Length];

        for (int i = 0; i < planetRequirements.Length; i++)
        {
            planetRequirements[i].button.SetCost(planetRequirements[i].energyRequired);
        }


        selectedStarColorIndex = DialogueLua.GetVariable("Vehicle Type").asInt;

        if (PlayerPrefs.GetInt("First Solar System Created", 0) == 0)
            {
                DialogueManager.StartConversation(firstConversation);
            }
        else
            {
                DialogueManager.StartConversation(conversation);
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

    public void Create()
    {
        solarFlares.GetComponent<SolarFlare>().SetCoreScale();

        //local scale should be 10

        float starSize = solarFlares.transform.localScale.x;
        Debug.Log("STAR SIZE: " + starSize);
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
        int remainingEnergy = EnergyAvailableToSpend();

        DialogueLua.SetVariable("Energy Available", remainingEnergy);
        PlayerPrefs.SetInt("First Solar System Created", 1);
        DialogueLua.SetVariable("Stars Created", DialogueLua.GetVariable("Stars Created").asInt + 1);

        for (int i = 0; i < planetRequirements.Length; i++)
        {
            planetAllocations[i] = planetRequirements[i].button.currentAmount;
        }

        GameObject go = Instantiate(starTemplate, transform);
        go.GetComponent<Renderer>().material.SetColor("_Color", starColors[selectedStarColorIndex]);
        Vector3 starScale = new Vector3(starSize, starSize, starSize);
        //scales template up by 10, multiplying for no reason?
//        go.transform.localScale = starScale;
        Vector2 starInfo = new Vector2(starSize, selectedStarColorIndex);
        Vector3 starCoordinates = new Vector3(0, 0, 0);
        PlayerPrefsX.SetVector3("solar_system_" + numberOfExistingSolarSystems + 1 + "_star_coordinates", starCoordinates);
        PlayerPrefsX.SetVector2("solar_system_" + numberOfExistingSolarSystems + 1 + "_star_info", starInfo);

        float distanceToStar = starScale.x;
        float padding = 1;
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
            //ASSIGN TYPE AND SIZE FOR EACH ALLOCATION
            for(int j = 0; j < planetAllocations[i]; j++)
            {
                //size
                planetInfo[currentIndex + j].y = planetRequirements[i].diameter;
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

        Debug.Log("New Solar System Created: " + DialogueLua.GetVariable("Ship Name").asString);

        //DESTROY SHIP FROM FLEET
        DestroyShip(DialogueLua.GetVariable("Selected Ship").asInt);

        string s = PersistentDataManager.GetSaveData(); // Save state.

    }

    public void DestroyShip(int shipId)
    {
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
