using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkingLot : MonoBehaviour
{

    public GameObject[] vehicles;
    public GameObject vehicle;
    public Feedback feedback;
    public int energyCollected;
    public float lightYearsTraveled;
    public AudioClip energyFx;
    public BoundaryPowerUp boundaries;
    public Damage HP;
    public UsePowerUps PowerUps;
    public int lightYearIncrement = 10;
//REMOVE?
    public Text lightyearUI;
    public Text planetUI;


    // Start is called before the first frame update
    void Start()
    {

    }


    public Vehicle GetVehicle()
    {
        return vehicle.GetComponentInChildren<Vehicle>();
       
    }

    public void SelfDestruct()
    {
        Time.timeScale = 1f;
        vehicle.GetComponentInChildren<Vehicle>().SelfDestruct();

    }
    public void EnergyCollected()
    {
        energyCollected++;
        GetComponent<AudioSource>().PlayOneShot(energyFx);

    }

    public void LightYearTraveled()
    {
        lightYearsTraveled += lightYearIncrement;

        if (lightYearsTraveled%100 == 0)
        {
            lightYearIncrement += 10;
        }
        GetComponent<AudioSource>().PlayOneShot(energyFx);

    }
    /*
    public void AddEnergy(int e)
    {
        energyCollected += e;
       // energyUI.text = energyCollected.ToString();
       // energyUI.gameObject.GetComponent<Animator>().SetTrigger("add");
        GetComponent<AudioSource>().PlayOneShot(energyFx);

    }
    */
    public void GiveFeedback(string message)
    {
        feedback.gameObject.SetActive(true);
        feedback.SetMessage(message);
    }

    public void SetEnergy()
    {
        //SET TOTAL SEEDS COLLECTED 
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(energyCollected, "CgkIm_nTr7sPEAIQAg", (bool success) =>
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
        PlayerPrefs.SetInt("energy", energyCollected + PlayerPrefs.GetInt("energy"));

    }

    public Vehicle DefaultVehicle()
    {
        vehicle = Instantiate(vehicles[0], transform);
        Vector3 newPosition = vehicle.transform.position;
        newPosition.z = 10f;
        vehicle.transform.position = newPosition;
        HP.SetHP(vehicle.GetComponentInChildren<Vehicle>());

        return vehicle.GetComponentInChildren<Vehicle>();
    }
    public Vehicle SelectVehicle(VehicleType v)
    {
        switch (v)
        {
            case VehicleType.Boomerang:
                vehicle = Instantiate(vehicles[0], transform);
                break;
            case VehicleType.Rocket:
                vehicle = Instantiate(vehicles[1], transform);
                break;

            case VehicleType.Racer:
                vehicle = Instantiate(vehicles[2], transform);
                break;

            case VehicleType.Falcon:
                vehicle = Instantiate(vehicles[3], transform);
                break;

            case VehicleType.Fighter:
                vehicle = Instantiate(vehicles[4], transform);
                break;

            case VehicleType.UFO:
                vehicle = Instantiate(vehicles[5], transform);

                break;

        }
        Vector3 newPosition = vehicle.transform.position;
        newPosition.z = 10f;
        vehicle.transform.position = newPosition;
        HP.SetHP(vehicle.GetComponentInChildren<Vehicle>());
        return vehicle.GetComponentInChildren<Vehicle>();
    }


}
