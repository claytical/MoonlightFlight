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

    void Start()
    {
        hangar = FindAnyObjectByType<Hangar>();
        transform.SetParent(hangar.setupScreen.transform);

        if (hangar)
        {
            AssignFirstAvailablePlane();
            SetVehicleStats();
        }
        else
        {
            Debug.LogError("Hangar not found.");
        }
        transform.localScale = new Vector3(1, 1, 1);

    }
    private void Update()
    {
    }
    private void SetVehicleStats()
    {
        if (chosenPlane && chosenPlane.GetComponent<Vehicle>())
        {
            SetStats(chosenPlane.GetComponent<Vehicle>());
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

        // Set fuel capacity based on vehicle's capabilities, if available
        int fuelCapacity = vehicle.capacity > 0 ? (int)vehicle.capacity : 50;
        fuel.FillTank(fuelCapacity);
        hp.SetHPUI(vehicle.currentHP);
    }

    public void SetVehicleIconColor(Color color)
    {
        planeIcon.color = color;
    }

    public void NextColor()
    {
        selectedColor++;
        if (selectedColor >= planeColors.Length)
        {
            selectedColor = 0;
        }
        SetVehicleIconColor(planeColors[selectedColor]);
    }

    public void PreviousColor()
    {
        selectedColor--;
        if (selectedColor < 0)
        {
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

            if (chosenPlane && chosenPlane.GetComponent<SpriteRenderer>())
            {
                planeIcon.sprite = chosenPlane.GetComponent<SpriteRenderer>().sprite;
                planeIcon.color = planeColors[selectedColor];
            }
            else
            {
                Debug.LogError("Chosen plane or SpriteRenderer not found.");
            }
        }
    }

    public void NextVehicle()
    {
        Debug.Log("Next Vehicle");
        vehicleId = hangar.NextAvailablePlane(vehicleId);
        chosenPlane = hangar.planes[vehicleId];

        if (chosenPlane && chosenPlane.GetComponent<SpriteRenderer>())
        {
            planeIcon.sprite = chosenPlane.GetComponent<SpriteRenderer>().sprite;
            SetVehicleStats();
        }
    }

    public void PreviousVehicle()
    {
        vehicleId = hangar.PreviousAvailableVehicle(vehicleId);
        chosenPlane = hangar.planes[vehicleId];

        if (chosenPlane && chosenPlane.GetComponent<SpriteRenderer>())
        {
            planeIcon.sprite = chosenPlane.GetComponent<SpriteRenderer>().sprite;
            SetVehicleStats();
        }
    }

    public GameObject GetChosenPlane()
    {
        return chosenPlane;
    }

    public void Confirm()
    {
        controlsAndStats.SetActive(false);
        planeIcon.transform.parent.gameObject.SetActive(false);
        border.enabled = false;
    }
}
