using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSlot : MonoBehaviour
{
    public Image HighlightMarker;
    public VehicleType vehicle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectShipSlot()
    {
        if(transform.parent.GetComponent<ShipControlPanel>())
            {
            transform.parent.GetComponent<ShipControlPanel>().ShipSlotInUse = this;
            transform.parent.GetComponent<ShipControlPanel>().ControlSubPanel.SetActive(true);
            transform.parent.GetComponent<ShipControlPanel>().gameObject.SetActive(false);
            Debug.Log("TRANSFORM IMAGE NAME: " + transform.GetComponent<Image>().sprite.name);
            transform.parent.GetComponent<ShipControlPanel>().ControlSubPanel.GetComponent<ShipSubControlPanel>().SelectedShip.sprite = transform.GetComponent<Image>().sprite;

            }
        else
            {
            Debug.Log("This slot doesn't have a parent control panel.");
            }
    }

}
