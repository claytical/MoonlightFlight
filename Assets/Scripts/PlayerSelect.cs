using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSelect : MonoBehaviour

{

    public Image planeIcon;
    public HP hp;
    public Fuel fuel;
    public Color[] planeColors;
    public GameObject controlsAndStats;
    public Image border;
    private int selectedColor = 0;
    private Hangar hangar;
    private int vehicleId;
    private GameObject chosenPlane;
    // Start is called before the first frame update
    void Start()
    {
        hangar = FindAnyObjectByType<Hangar>();
        transform.parent = hangar.setupScreen.transform;
        if(hangar)
        {
            AssignFirstAvailablePlane();
            if(chosenPlane.GetComponent<Vehicle>())
            {
                SetStats(chosenPlane.GetComponent<Vehicle>());

            }
        }
        else
        {

        }
    }

    public void SetStats(Vehicle vehicle)
    {
        foreach (Transform child in hp.hpLeft.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in fuel.tank)
        {
            Destroy(child.gameObject);
        }
        fuel.FillTank(5);
        hp.SetHPUI(vehicle.currentHP, vehicle.maxHP);
    }

    public void SetVehicleIconColor(Color color)
    {
        planeIcon.color = color;
    }
    
    public void NextColor()
    {
        selectedColor++;
        if(selectedColor >= planeColors.Length)
        {
            selectedColor = 0;
        }
        SetVehicleIconColor(planeColors[selectedColor]);
    }
    public void PreviousColor()
    {
       selectedColor--;
       if (selectedColor < 0) {
            selectedColor = planeColors.Length - 1;
       }
       SetVehicleIconColor(planeColors[selectedColor]);

    }

    private void AssignFirstAvailablePlane()
    {

        if (hangar != null)
        {
            vehicleId = hangar.FirstAvailablePlane();

            chosenPlane = hangar.planes[vehicleId];

            planeIcon.sprite = hangar.planes[vehicleId].GetComponent<SpriteRenderer>().sprite;
            planeIcon.color = planeColors[selectedColor];
        }

    }
    public void NextVehicle()
    {
        Debug.Log("Next");
            vehicleId = hangar.NextAvailablePlane(vehicleId);
            chosenPlane = hangar.planes[vehicleId];
            planeIcon.sprite = hangar.planes[vehicleId].GetComponent<SpriteRenderer>().sprite;
            if (chosenPlane.GetComponent<Vehicle>())
            {
                SetStats(chosenPlane.GetComponent<Vehicle>());
            }
    }

    public void PreviousVehicle()
    {
            vehicleId = hangar.PreviousAvailableVehicle(vehicleId);
            chosenPlane = hangar.planes[vehicleId];
            planeIcon.sprite = hangar.planes[vehicleId].GetComponent<SpriteRenderer>().sprite;
            if (chosenPlane.GetComponent<Vehicle>())
            {
                SetStats(chosenPlane.GetComponent<Vehicle>());

            }
    }

    public GameObject GetChosenPlane()
    {
        return chosenPlane;
    }
    public void Confirm()
    {
            controlsAndStats.SetActive(false);
            border.enabled = false;
    }


}
