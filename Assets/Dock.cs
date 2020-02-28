using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dock : MonoBehaviour
{

    public GameObject[] ships;
    public GameObject ship;
    public int seedsCollected;
    public Text seedUI;
    public AudioClip seedFx;
    public BoundaryPowerUp boundaries;
    private string shipName;

    // Start is called before the first frame update
    void Start()
    {

    }


    public Ship GetShip()
    {
        return ship.GetComponentInChildren<Ship>();
    }

    public void AddSeeds(int seeds)
    {
        seedsCollected += seeds;
        seedUI.text = seedsCollected.ToString();
        seedUI.gameObject.GetComponent<Animator>().SetTrigger("add");
        GetComponent<AudioSource>().PlayOneShot(seedFx);
    }


    public void SetSeeds()
    {
        //SET SHIP SPECIFIC SEEDS COLLECTED
        PlayerPrefs.SetInt(shipName + "_seeds", seedsCollected + PlayerPrefs.GetInt(shipName + "_seeds"));
        //SET TOTAL SEEDS COLLECTED 
        PlayerPrefs.SetInt("seeds", seedsCollected + PlayerPrefs.GetInt("seeds"));

    }

    
    public Ship SelectShip(GameState gs)
    {
        switch (gs.ship)
        {
            case GameState.Ship.Boomerang:
                ship = Instantiate(ships[0], transform);
                shipName = "boomerang";
                break;

            case GameState.Ship.Falcon:
                ship = Instantiate(ships[3], transform);
                shipName = "falcon";
                break;

            case GameState.Ship.Fighter:
                shipName = "fighter";
                ship = Instantiate(ships[4], transform);
                break;

            case GameState.Ship.Rocket:
                ship = Instantiate(ships[1], transform);
                shipName = "rocket";
                break;

            case GameState.Ship.UFO:
                shipName = "ufo";
                ship = Instantiate(ships[5], transform);

                break;

            case GameState.Ship.XWing:
                shipName = "xwing";
                ship = Instantiate(ships[2], transform);
                break;

        }
        Vector3 newPosition = ship.transform.position;
        newPosition.z = 10f;
        ship.transform.position = newPosition;
        return ship.GetComponentInChildren<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
    }



}
