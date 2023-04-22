using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Memorial : MonoBehaviour
{

    public Fleet fleet;
    public GameObject tombstone;
    public Transform t;
    private List<string> destroyedShipNames;
    private List<int> destroyedShipTypes;
    private List<int> destroyedShipEnergies;

    // Start is called before the first frame update
    void Start()
    {
        destroyedShipNames = PlayerPrefsX.GetStringArray("Destroyed Ship Names").ToList();
        destroyedShipTypes = PlayerPrefsX.GetIntArray("Destroyed Ship Types").ToList();
        destroyedShipEnergies = PlayerPrefsX.GetIntArray("Destroyed Ship Energy").ToList();
        for(int i = 0; i < destroyedShipNames.Count; i++)
        {
            GameObject go = Instantiate(tombstone, t);
            go.GetComponent<Tombstone>().shipName.text = destroyedShipNames[i];
            go.GetComponent<Tombstone>().energyCollected.text = destroyedShipEnergies[i].ToString("0");
            go.GetComponent<Tombstone>().shipType.sprite = fleet.vehiclesAvailable[destroyedShipTypes[i]].gameObject.GetComponent<Image>().sprite;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
