using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public enum VehicleType
{
    /*
    Boomerang = 0,
    Rocket = 1,
    Racer = 2,
    Falcon = 3,
    Fighter = 4,
    UFO = 5,
    Butterfly = 6
*/
    A = 0,
    AA = 1,
    FF = 2,
    M = 3,
    NN = 4,
    SS = 5,
    XX = 6,
    ZZ = 7

};

public class VehicleStats : MonoBehaviour
{
    public VehicleType type;
    public GameObject vehicle;
    private Vehicle _vehicle;

    public GameObject speedMeter;
    private Image[] speed;

    public GameObject accelerationMeter;
    private Image[] acceleration;

    public Color energyColor;

    // Start is called before the first frame update

    private float map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    void Start()
    {
        Debug.Log("AM I BEING USED?");
        /*
        _vehicle = vehicle.GetComponentInChildren<Vehicle>();
        if (!_vehicle)
        {
            Debug.Log("This prefab doesn't have a Vehicle script attached to it.");
        }
        else
        {
            acceleration = accelerationMeter.GetComponentsInChildren<Image>();
            int vehicleAcceleration = (int)map(0, 15, 0, acceleration.Length, _vehicle.force);

            speed = speedMeter.GetComponentsInChildren<Image>();

            int vehicleSpeed = (int) map(0, 30, 0, speed.Length, _vehicle.maxForce);


            for (int i = 0; i < vehicleSpeed; i++)
            {
                speed[i].color = energyColor;
            }

            for (int i = 0; i < vehicleAcceleration; i++)
            {
                acceleration[i].color = energyColor;
            }
        }
        */
    }
}
