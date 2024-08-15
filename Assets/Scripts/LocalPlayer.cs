using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LocalPlayer : MonoBehaviour
{
    public GameObject playerSelect;
    public GameObject playerVehicle;
    public GameObject playerStats;
    public AudioClip clickFx;
    public AudioClip confirmFx;

    private Vehicle plane;
    private PlayerSelect selection;
    private PlayerStats ui;
    private Color color;
    private bool isReady = false;
    public bool IsReady => isReady; // Public read-only property

    void Start()
    {
        DontDestroyOnLoad(this);
        GamepadManager.Instance.RegisterPlayer(this);
        CreatePlayerSelect();
    }

    public void CreatePlayerSelect()
    {
        GameObject ps = Instantiate(playerSelect);
        if (ps.GetComponent<PlayerSelect>())
        {
            selection = ps.GetComponent<PlayerSelect>();
        }
    }

    public void SetStats()
    {
        ui.SetStats(plane);
    }

    public void SetColor()
    {
        color = selection.planeIcon.color;
    }

    public void Launch()
    {
        // ADD UI
        GameStatsUI gameStats = FindAnyObjectByType<GameStatsUI>();

        GameObject stats = Instantiate(playerStats, gameStats.transform);
        ui = stats.GetComponent<PlayerStats>();

        // ADD PLANE
        GameObject vehicle = Instantiate(playerVehicle, transform);
        if (vehicle.GetComponent<Vehicle>())
        {
            plane = vehicle.GetComponent<Vehicle>();

            // Set the plane color
            if (plane.GetComponent<SpriteRenderer>())
            {
                plane.GetComponent<SpriteRenderer>().color = color;
            }

            // Assign Fuel UI to the vehicle
            plane.fuelUI = ui.GetComponentInChildren<Fuel>();
            plane.energyCollected = plane.capacity;
            plane.playerStats = GetComponent<PlayerStatsTracking>();
            SetStats();
            ui.SetColor(color);
            plane.Fly();

            GetComponent<PlayerInput>().SwitchCurrentActionMap("Play");
        }
    }

    public void Restart()
    {
        isReady = false;
        ui.Deactivate();
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Start");
        GamepadManager.Instance.CheckAllPlayersGone();
    }

    public bool TakeDamage(int damage)
    {
        StartRumble(.25f, .5f, 0.5f);
        return ui.TakeDamage(damage);
    }

    public void EnergyCollected(int newAmount)
    {
        ui.EnergyCollected(newAmount);
    }

    public void StartRumble(float lowFrequency, float highFrequency, float duration)
    {
        var gamepad = GetComponent<PlayerInput>().devices[0] as Gamepad;
        if (gamepad != null)
        {
            Debug.Log("RUMBLE!");
            GamepadManager.Instance.StartRumble(gamepad, lowFrequency, highFrequency, duration);
        }
        else
        {
            Debug.Log("No gamepad for rumbling");
        }
    }

    public void OnMove(InputValue value)
    {
        if (plane != null)
        {
            plane.Move(value.Get<Vector2>().normalized);
        }
    }

    public void OnThrust(InputValue value)
    {
        if (plane != null)
        {
            if (value.isPressed)
            {
                Debug.Log("Thrust Started");
                if (!plane.IsBoosting())
                {
                    plane.TurnOnBoost();
                }
            }
            else
            {
                Debug.Log("Thrust Stopped");
                plane.TurnOffBoost();
            }
        }
    }

    public void OnNextColor(InputValue value)
    {
        if (value.isPressed)
        {
            GetComponent<AudioSource>().PlayOneShot(clickFx);
            selection.NextColor();
        }
    }

    public void OnPreviousColor(InputValue value)
    {
        if (value.isPressed)
        {
            GetComponent<AudioSource>().PlayOneShot(clickFx);
            selection.PreviousColor();
        }
    }

    public void OnNextVehicle(InputValue value)
    {
        if (value.isPressed)
        {
            GetComponent<AudioSource>().PlayOneShot(clickFx);
            selection.NextVehicle();
        }
    }

    public void OnPreviousVehicle(InputValue value)
    {
        if (value.isPressed)
        {
            GetComponent<AudioSource>().PlayOneShot(clickFx);
            selection.PreviousVehicle();
        }
    }

    public void OnChoose(InputValue value)
    {
        Debug.Log("Confirmed Selection");
        switch (SceneManager.GetActiveScene().name)
        {
            case "Track":
                break;
            case "TrackSelection":
                GetComponent<AudioSource>().PlayOneShot(confirmFx);
                playerVehicle = selection.GetChosenPlane();
                selection.Confirm();
                SetColor();
                isReady = !isReady; // Toggle ready state
                Debug.Log($"Player {gameObject.name} is {(isReady ? "ready" : "not ready")}");
                GamepadManager.Instance.CheckAllPlayersReady();
                break;
            case "TrackFinished":
                GetComponent<AudioSource>().PlayOneShot(confirmFx);
                GamepadManager.Instance.LoadNewScene("TrackSelection");
                break;
        }
    }
}
