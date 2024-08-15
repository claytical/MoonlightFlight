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
    // Start is called before the first frame update

    void Start() {}
    private void Update() {}

    public void PlayerJoined(int playerId)
    {
        Debug.Log("PLAYER " + playerId + " JOINED!");
        intro.SetActive(false);
    }
}
   
