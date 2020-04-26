using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dock : MonoBehaviour
{

    public GameObject[] ships;
    public GameObject ship;
    public Feedback feedback;
    public int seedsCollected;
    public Text seedUI;
    public AudioClip seedFx;
    public BoundaryPowerUp boundaries;
 
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

    public void GiveFeedback(string message)
    {
        feedback.gameObject.SetActive(true);
        feedback.SetMessage(message);
    }

    public void SetSeeds()
    {
        //SET TOTAL SEEDS COLLECTED 
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(seedsCollected, "CgkIm_nTr7sPEAIQAg", (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Update Score Success");

                }
                else
                {
                    Debug.Log("Update Score Fail");
                }
            });
        }
        PlayerPrefs.SetInt("seeds", seedsCollected + PlayerPrefs.GetInt("seeds"));

    }

    
    public Ship SelectShip(ShipType s)
    {
        switch (s)
        {
            case ShipType.Boomerang:
                ship = Instantiate(ships[0], transform);
                break;
            case ShipType.Rocket:
                ship = Instantiate(ships[1], transform);
                break;

            case ShipType.Racer:
                ship = Instantiate(ships[2], transform);
                break;

            case ShipType.Falcon:
                ship = Instantiate(ships[3], transform);
                break;

            case ShipType.Fighter:
                ship = Instantiate(ships[4], transform);
                break;

            case ShipType.UFO:
                ship = Instantiate(ships[5], transform);

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
