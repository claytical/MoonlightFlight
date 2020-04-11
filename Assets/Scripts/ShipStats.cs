using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public enum ShipType
{
    Boomerang = 0,
    Rocket = 1,
    Racer = 2,
    Falcon = 3,
    Fighter = 4,
    UFO = 5
};

public class ShipStats : MonoBehaviour
{
    public ShipType type;
    public GameObject ship;
    private Ship _ship;

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
        _ship = ship.GetComponentInChildren<Ship>();
        if (!_ship)
        {
            Debug.Log("This prefab doesn't have a Ship script attached to it.");
        }
        else
        {
            acceleration = accelerationMeter.GetComponentsInChildren<Image>();
            int shipAcceleration = (int)map(0, 15, 0, acceleration.Length, _ship.force);

            speed = speedMeter.GetComponentsInChildren<Image>();

            int shipSpeed = (int) map(0, 30, 0, speed.Length, _ship.maxForce);


            for (int i = 0; i < shipSpeed; i++)
            {
                speed[i].color = energyColor;
            }

            for (int i = 0; i < shipAcceleration; i++)
            {
                acceleration[i].color = energyColor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
