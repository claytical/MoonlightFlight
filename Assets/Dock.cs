using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour
{

    public GameObject[] ships;
    public GameObject ship;
    public int seedsCollected;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void AddSeeds(int seeds)
    {
        seedsCollected += seeds;
    }

    public void SelectShip(GameState gs)
    {
        switch (gs.ship)
        {
            case GameState.Ship.Boomerang:
                ship = Instantiate(ships[0], transform);
                break;

            case GameState.Ship.Rocket:
                ship = Instantiate(ships[1], transform);
                break;

            case GameState.Ship.XWing:
                ship = Instantiate(ships[2], transform);
                break;

            case GameState.Ship.Falcon:
                ship = Instantiate(ships[3], transform);
                break;

            case GameState.Ship.Shooter:
                ship = Instantiate(ships[4], transform);
                break;

            case GameState.Ship.UFO:
                ship = Instantiate(ships[5], transform);

                break;

        }


    }

    // Update is called once per frame
    void Update()
    {
    }



}
