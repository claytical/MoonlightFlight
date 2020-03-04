using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipUnlockPanel : MonoBehaviour
{
    public GameObject shipContainer;
    public ShipStats[] ships;
    // Start is called before the first frame update
    void Start()
    {
        ships = shipContainer.GetComponentsInChildren<ShipStats>();
        for(int i = 0; i < ships.Length; i++)
        {
            ships[i].gameObject.SetActive(false);
        }
        //turn off panel
        gameObject.SetActive(false);
    }

    public void UnlockShip(ShipType type)
    {
        for(int i = 0; i < ships.Length; i++)
        {
            if(ships[i].type == type)
            {
                Debug.Log("Unlocking " + type);
                ships[i].gameObject.SetActive(true);
                break;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
