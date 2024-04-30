using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Players : MonoBehaviour
{
    public GameObject intro;
    //public TextMeshProUGUI startText;
    public GameObject spawnLocations;
    public ProceduralLevel level;
    public Boundaries boundaries;
    public int countdownTime = 10;
    private float countdownTimer = 999999;
    private bool countdown = false;
    private bool gameInProgress = false;
    // Start is called before the first frame update
    void Start()
    {
        intro.SetActive(true);
    }

    private void Update()
    {
        if (countdownTimer <= Time.time && countdown)
        {
            countdown = false;
            intro.SetActive(false);
            gameInProgress = true;
            //queue first set
            level.Play();
        }

        if(!gameInProgress)
        {
            if (countdown)
            {
      /*
                if (startText)
                {
                    startText.text = (countdownTimer - Time.time).ToString("0");
                }
                else
                {
                    Debug.Log("no start ttext found");
                }
      */
                }
      
        }

    }

    public void PlayerJoined(int playerId)
    {
        Debug.Log("PLAYER " + playerId + " JOINED!");

        if (!gameInProgress)
        {
            intro.SetActive(false);
            countdown = false;
        }
    }
    public void PlayerChoseVehicle(int playerId)
    {
        if(!gameInProgress)
        {
            //intro.SetActive(true);
            countdown = true;
            switch (playerId)
            {
                case 1:
                case 2:
                case 3:
                    countdownTimer = Time.time + countdownTime;
                    Debug.Log("COUNTDOWN TIMER: " + countdownTimer);
                    //COUNTDOWN
                    break;
                case 4:
                    break;

            }

        }
    }

}
