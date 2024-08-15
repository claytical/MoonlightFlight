using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GamepadManager : MonoBehaviour
{

    public GameObject introScreen;
    public GameObject gameSetupScreen;
    public Hangar hangar;
    // List to store the names of the connected gamepads
    private List<Gamepad> connectedGamepads;
    private List<LocalPlayer> localPlayers = new List<LocalPlayer>();
    public static GamepadManager Instance { get; private set; }

    public ProceduralLevel level;

    public int countdownTime = 10;
    private float countdownTimer = 999999;
    private bool countdown = false;
    private bool gameInProgress = false;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        connectedGamepads = new List<Gamepad>();
        foreach (var gamepad in Gamepad.all)
        {
            connectedGamepads.Add(gamepad);
        }
    }

    private void Update()
    {
        if (countdownTimer <= Time.time && countdown)
        {
            countdown = false;
            gameInProgress = true;
            //queue first set
            level.Play();
        }

    }

    public void RegisterPlayer(LocalPlayer player)
    {
        localPlayers.Add(player);
    }

    public void CheckAllPlayersReady()
    {
        foreach (LocalPlayer player in localPlayers)
        {
            if (!player.IsReady)
            {
                return; // Exit if any player is not ready
            }
        }

        StartGame(); // All players are ready
    }

    public void CheckAllPlayersGone()
    {
        foreach (LocalPlayer player in localPlayers)
        {
            if (player.IsReady)
            {
                return; // Exit if any player is not ready
            }
        }
        //ALL PLAYERS GONE
        Debug.Log("Game Over");        
        LoadNewScene("TrackFinished");

    }

    private void StartGame()
    {
        Debug.Log("All players are ready. Starting the game!");
        // Implement game start logic here
        LoadNewScene("Track");
    }


    public void JoinGame(PlayerInput playerInput)
    {
        if (introScreen.activeSelf)
        {
            introScreen.SetActive(false);
            gameSetupScreen.SetActive(true);
        }
        // Assign the joined gamepad to the player
        var gamepad = playerInput.devices[0] as Gamepad;
        if (gamepad != null && !connectedGamepads.Contains(gamepad))
        {
            connectedGamepads.Add(gamepad);
        }
    }

    public void LeaveGame(PlayerInput playerInput)
    {
        Debug.Log(playerInput.GetInstanceID().ToString("PLAYER 0 LEFT"));
        // Remove the left gamepad from the list
        var gamepad = playerInput.devices[0] as Gamepad;
        if (gamepad != null && connectedGamepads.Contains(gamepad))
        {
            connectedGamepads.Remove(gamepad);
        }

    }

    public string CurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }
    public void LoadNewScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene has finished loading
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Run your code here

        Debug.Log("New scene loaded: " + sceneName);
        // Implement any additional logic here
        if(sceneName.Equals("Track"))
        {
            level = FindAnyObjectByType<ProceduralLevel>();
            for (int i = 0; i < localPlayers.Count; i++)
            {
                localPlayers[i].Launch();

            }
            countdown = true;
            countdownTimer = Time.time + countdownTime;
            Debug.Log("COUNTDOWN TIMER: " + countdownTimer);

            /*
                    if (spawnLocations[playerId])
                    {
                        chosenVehicle.transform.position = spawnLocations[playerId].position;
                    }
            */
        }
        if(sceneName.Equals("TrackFinished"))
        {
            Debug.Log("Track Finished! Show Stats");
            //RESTART ON BUTTON, POPULATE WITH PREVIOUS CONFIG
        }
        if (sceneName.Equals("TrackSelection"))
        {
            for (int i = 0; i < localPlayers.Count; i++)
            {
                localPlayers[i].CreatePlayerSelect();
                localPlayers[i].SetStats();
            }

        }

    }
    public void StartRumble(Gamepad gamepad, float lowFrequency, float highFrequency, float duration)
    {

        if (gamepad == null)
        {
            Debug.Log("Gamepad is null");
            return;
        }
        else
        {
            Debug.Log("gamepad is not null, let's rumble");
        }
        gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
        StartCoroutine(StopRumbleAfterDuration(gamepad, duration));
    }

    
    // Method to start rumble on the player's gamepad
    public void StartRumble(float lowFrequency, float highFrequency, float duration)
    {
        var device = GetComponent<PlayerInput>().devices[0];

        if (device is UnityEngine.InputSystem.Switch.SwitchProControllerHID switchProController)
        {
            switchProController.SetMotorSpeeds(lowFrequency, highFrequency);
            StartCoroutine(StopRumble(switchProController, duration));
        }
    }

    // Coroutine to stop rumble after a duration
    private IEnumerator StopRumble(UnityEngine.InputSystem.Switch.SwitchProControllerHID switchProController, float duration)
    {
        yield return new WaitForSeconds(duration);
        switchProController.SetMotorSpeeds(0, 0);
    }


    private IEnumerator StopRumbleAfterDuration(Gamepad gamepad, float duration)
    {
        yield return new WaitForSeconds(duration);
        gamepad.SetMotorSpeeds(0, 0);
    }

    // Example: Trigger rumble for all connected gamepads when called
    public void TriggerRumbleForAll(float lowFrequency, float highFrequency, float duration)
    {
        foreach (var gamepad in connectedGamepads)
        {
            StartRumble(gamepad, lowFrequency, highFrequency, duration);
        }
    }


}
