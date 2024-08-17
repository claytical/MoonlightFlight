using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LocalPlayer : MonoBehaviour
{
    public GameObject playerSelect;
    public GameObject playerVehicle;
    public GameObject playerStatsUI; // Renamed to clarify that this is the UI
    public AudioClip clickFx;
    public AudioClip confirmFx;

    private Vehicle plane;
    private PlayerSelect selection;
    private PlayerStatsTracking playerStats; // Responsible for tracking metrics
    private PlayerStats ui; // Responsible for managing the player's UI
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

        GameObject statsUI = Instantiate(playerStatsUI, gameStats.transform);
        ui = statsUI.GetComponent<PlayerStats>(); // Assign PlayerStats (UI)

        // ADD PLANE
        GameObject vehicle = Instantiate(playerVehicle, transform);
        playerStats = GetComponent<PlayerStatsTracking>();
        if (vehicle.GetComponent<Vehicle>())
        {
            plane = vehicle.GetComponent<Vehicle>();

            // PlayerStatsTracking component is attached to the plane
            plane.playerStats = playerStats;// = plane.GetComponent<PlayerStatsTracking>();
            plane.playerStatsUI = ui;
            if (plane.GetComponent<SpriteRenderer>())
            {
                plane.GetComponent<SpriteRenderer>().color = color;
            }
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
            GamepadManager.Instance.StartRumble(gamepad, lowFrequency, highFrequency, duration);
        }
    }

    public void OnMove(InputValue value)
    {
        plane.Move(value.Get<Vector2>().normalized);
    }

    public void OnThrust(InputValue value)
    {
        float triggerValue = value.Get<float>(); // Get the right trigger value (0 to 1)

        if (triggerValue > 0)
        {
            plane.TurnOnBoost(triggerValue); // Pass trigger value to control thrust
        }
        else
        {
            plane.TurnOffBoost(); // Stop boosting when trigger is released
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
