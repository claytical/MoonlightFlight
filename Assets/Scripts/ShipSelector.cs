using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelector : MonoBehaviour
{
    private List<GameObject> ships;
    private int currentShipIndex = 0;
    public ShipType selectedShip;
    public GameState gs;

    // Start is called before the first frame update
    void Start()
    {
        int galaxies = PlayerPrefs.GetInt("galaxies", 0);
        if(galaxies <= 0)
        {

        }
        else {

            Debug.Log("Not processing ship types yet...");
            ships = new List<GameObject>();
            /*
            for (int i = 0; i < GetComponentsInChildren<ShipStats>().Length; i++)
            {
                ships.Add(GetComponentsInChildren<ShipStats>()[i].gameObject);
                //remove existing lock check
                PlayerPrefs.DeleteKey(GetComponentsInChildren<ShipStats>()[i].type.ToString());
                int seedsNeeded = GetComponentsInChildren<ShipLock>()[i].seedsRequired - PlayerPrefs.GetInt("seeds", 0);
                if (seedsNeeded > 0)
                {
                    //ship locked
                    PlayerPrefs.SetInt(GetComponentsInChildren<ShipStats>()[i].type.ToString(), GetComponentsInChildren<ShipLock>()[i].seedsRequired);
                    Debug.Log("STILL LOCKED: " + GetComponentsInChildren<ShipStats>()[i].type.ToString());
                }

            }
            Debug.Log(ships.Count + " ships found");
            toggleShips();
            */
        }

    }

    private void toggleShips()
    {
        for (int i = 0; i < ships.Count; i++)
        {
            ships[i].SetActive(false);
        }
        //turn on first ship
        ships[currentShipIndex].SetActive(true);
        selectedShip = ships[currentShipIndex].GetComponent<ShipStats>().type;
        if(ships[currentShipIndex].GetComponent<ShipLock>().SetLock())
        {
            gs.SetShip(selectedShip);
        }
    }


    public void Previous()
    {
        currentShipIndex--;
        if (currentShipIndex < 0)
        {
            currentShipIndex = ships.Count - 1;
        }
        toggleShips();

    }
    public void Next()
    {
        currentShipIndex++;
        if(currentShipIndex >= ships.Count)
        {
            currentShipIndex = 0;
        }
        toggleShips();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
