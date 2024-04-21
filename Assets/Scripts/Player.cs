using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int playerId;
    private int vehicleId;
    public GameObject chosenVehicle;
    public GameObject playerStats;
    private float timerTimer = 0;
    private bool isSlowingDown = false;
    private bool isSpeedingUp = false;
    private float targetTimeScale = 1f;
    private float currentTimeScale = 1f;
    private float lerpStartTime;
    private bool hyperBrake = false;
    private float hyperBrakeTimer;
    private bool thrust = false;
    private float thrustTimer;
    public float transitionDuration = 1f;
    private ParkingLot parkingLot;
    private bool firstPlay = true;

    private void Start()
    {
        //FIND POSSIBLE VEHICLES
        parkingLot = FindObjectOfType<ParkingLot>();
        playerId = PlayerInputManager.instance.playerCount;
        playerStats = Instantiate(playerStats, PlayerInputManager.instance.gameObject.transform);
        playerStats.GetComponentInParent<Players>().PlayerJoined(playerId);

        AssignFirstVehicleInParkingLot();
    }


    public void Restart()
    {

        //        playerStats. //energy
        playerStats.GetComponent<PlayerStats>().Deactivate();
        SwitchActionMap("Start");
        
    }

    private void AssignFirstVehicleInParkingLot()
    {

        if (parkingLot != null)
        {
            // Do something with the ParkingLot component or its GameObject
            Debug.Log("Found ParkingLot: " + parkingLot.gameObject.name);

            vehicleId = parkingLot.FirstAvailableVehicle();
            chosenVehicle = parkingLot.vehicles[vehicleId];
            playerStats.GetComponent<PlayerStats>().vehicleIcon.sprite = parkingLot.vehicles[vehicleId].GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            Debug.LogWarning("No ParkingLot found in the scene.");
        }

    }

    public void NextVehicle(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            vehicleId = parkingLot.NextAvailableVehicle(vehicleId);
            chosenVehicle = parkingLot.vehicles[vehicleId];
            playerStats.GetComponent<PlayerStats>().vehicleIcon.sprite = parkingLot.vehicles[vehicleId].GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void PreviousVehicle(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            vehicleId = parkingLot.PreviousAvailableVehicle(vehicleId);
            chosenVehicle = parkingLot.vehicles[vehicleId];
            playerStats.GetComponent<PlayerStats>().vehicleIcon.sprite = parkingLot.vehicles[vehicleId].GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void SwitchActionMap(string map)
    {
        firstPlay = false;
        GetComponent<PlayerInput>().SwitchCurrentActionMap(map);
    }

    public void ChooseVehicle(InputAction.CallbackContext value)
    {
//        AssignFirstVehicleInParkingLot();

        if (value.started)
        {
            if (chosenVehicle)
            {
                Transform[] spawnLocations = playerStats.GetComponentInParent<Players>().spawnLocations.GetComponentsInChildren<Transform>();

                if (playerStats.GetComponentInParent<Players>())
                {
                    playerStats.GetComponentInParent<Players>().PlayerChoseVehicle(playerId);
                }
                SwitchActionMap("Play");
                chosenVehicle = Instantiate(chosenVehicle,transform);
                if(spawnLocations[playerId])
                {
                    chosenVehicle.transform.position = spawnLocations[playerId].position;
                }
                chosenVehicle.GetComponent<Vehicle>().Fly();
                playerStats.GetComponent<PlayerStats>().SetStats(chosenVehicle.GetComponent<Vehicle>());

            }
            else
            {
                Debug.Log("No available vehicles!");
            }

        }

    }

    void FixedUpdate()
    {
        if (isSlowingDown || isSpeedingUp)
        {
            float lerpProgress = (Time.time - lerpStartTime) / transitionDuration;
            currentTimeScale = Mathf.Lerp(currentTimeScale, targetTimeScale, lerpProgress);

            if (lerpProgress >= 1f)
            {
                isSlowingDown = false;
                isSpeedingUp = false;
                Time.timeScale = targetTimeScale;
            }
            else
            {
                Time.timeScale = currentTimeScale;
            }
        }

        if (timerTimer < Time.time)
        {
            ToggleNormalTime();
        }


        if(chosenVehicle)
        {
//            chosenVehicle.GetComponent<Rigidbody2D>().AddForce(newForce, ForceMode2D.Impulse);
            if (hyperBrake)
            {
                if (hyperBrakeTimer > Time.time)
                {
                    Color c = chosenVehicle.GetComponent<SpriteRenderer>().color;
                    c.a = .5f;
                    chosenVehicle.GetComponent<SpriteRenderer>().color = c;
                    chosenVehicle.GetComponent<Rigidbody2D>().drag = 1f;
    
                }
                else
                {
                    Color c = chosenVehicle.GetComponent<SpriteRenderer>().color;
                    c.a = .75f;
                    chosenVehicle.GetComponent<SpriteRenderer>().color = c;
                    chosenVehicle.GetComponent<Rigidbody2D>().drag = 0f;
                    Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift = 0f;
                    hyperBrake = false;
                }

            }
            else if (thrust)
            {
                Color c = chosenVehicle.GetComponent<SpriteRenderer>().color;
                c.a = 1f;
                chosenVehicle.GetComponent<SpriteRenderer>().color = c;
                Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().scanLineJitter = .2f;

                if (thrustTimer < Time.time)
                {
                    c.a = .75f;
                    chosenVehicle.GetComponent<SpriteRenderer>().color = c;
                    Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().scanLineJitter = 0;
                    thrust = false;
                    chosenVehicle.GetComponent<Vehicle>().terminalVelocity /= chosenVehicle.GetComponent<Vehicle>().boost;
                    chosenVehicle.GetComponent<Vehicle>().force /= chosenVehicle.GetComponent<Vehicle>().boost;

                }
            }


        }


        /*
        if (isDead && !deadShip)
        {
            Debug.Log("DEAD SHIP. GAME OVER");
            Vector3 pos = transform.position;
            Instantiate(explosion, pos, Quaternion.identity);
            if (level)
            {
                level.GameOver();
            }
            this.gameObject.SetActive(false);
            deadShip = true;
        }
        */


    }



    public void Move(InputAction.CallbackContext value)
    {
        chosenVehicle.GetComponent<Vehicle>().Move(value.ReadValue<Vector2>().normalized);
//        chosenVehicle.GetComponent<Rigidbody2D>().AddForce(, ForceMode2D.Impulse);
    }
    public void ApplyThrusters()
    {
        if(!thrust)
        {
            Debug.Log("Thrusting");
            chosenVehicle.GetComponent<Vehicle>().force *= chosenVehicle.GetComponent<Vehicle>().boost;
            chosenVehicle.GetComponent<Vehicle>().terminalVelocity *= chosenVehicle.GetComponent<Vehicle>().boost;
            thrust = true;
            thrustTimer = Time.time + .5f;

        }

    }

    public void ToggleSlowMotion(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Slow Motion!");
            timerTimer = Time.time + 1f;
            if (!isSlowingDown)
            {
                targetTimeScale = .3f;
                lerpStartTime = Time.time;
                isSlowingDown = true;
            }

        }
    }
    public void EngageThrusters()
    {
  //      maxForce = initialForce * 2f;
  //      force = maxForce;
  //      Debug.Log("FORCE: " + force);
        //        thrusterSetLength
    }

    public void CheckThrusters()
    {
        /*
        if (thrusterSetCount >= thrusterSetLength)
        {
            DisengageThrusters();
        }
        else
        {
            thrusterSetCount++;
        }
        */
    }

    private void DisengageThrusters()
    {
    //    maxForce /= 2;
    //    force = initialForce;

    }

    public void ToggleDoubleTime()
    {
        timerTimer = Time.time + 1f;

        if (!isSpeedingUp)
        {
            targetTimeScale = 2f;
            lerpStartTime = Time.time;
            isSpeedingUp = true;
        }
    }

    public void ToggleNormalTime()
    {
        timerTimer = Time.time + 1f;
        if (!isSlowingDown && !isSpeedingUp)
        {
            targetTimeScale = 1f;
            lerpStartTime = Time.time;
            isSlowingDown = true;
        }
    }

    public void ApplyHyperBrake()
    {
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift = .05f;
        Color c = chosenVehicle.GetComponent<SpriteRenderer>().color;
        c.a = 75f;
        chosenVehicle.GetComponent<SpriteRenderer>().color = c;
        hyperBrake = true;
        hyperBrakeTimer = Time.time + .5f;
    }


}
