using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    public GameObject hp;
    public GameObject unitForGauge;

    public Transform gauge;
    public Transform hpLeft;

    public BoundaryPowerUp boundaries;
    public GameObject hull;

    private float speed = .01f;
    private float amount = .01f;

    private float hitDamageTime;
    private Vector3 originalPosition;
    private int maxHP;


    // Use this for initialization
    void Start()
    {
        originalPosition = transform.position;
    }

    public void SetHP(Vehicle vehicle)
    {
        Debug.Log("Setting HP for " + vehicle.name);
        maxHP = vehicle.maxHP;
        for (int i = 0; i < vehicle.maxHP; i++)
        {
            GameObject container = Instantiate(unitForGauge, gauge);
        }

        for (int i = 0; i < vehicle.currentHP; i++)
        {
            GameObject hpUnit = Instantiate(hp, hpLeft);
        }

    }

    public void IncreaseHP()
    {
        if(hpLeft.GetComponentsInChildren<Transform>().Length < maxHP)
        {
            GameObject hpUnit = Instantiate(hp, hpLeft);

        }
        else
        {
            Debug.Log("Max Armor Hit");
        }
    }

    public bool TakeDamage()
    {
        if(hull.GetComponent<Animator>()) {
            if(!hull.GetComponent<Animator>().IsInTransition(0))
            {
                hull.GetComponent<Animator>().SetTrigger("hit");

            }
        }
        if (hpLeft.GetComponentsInChildren<Image>().Length > 1)
        {
            Destroy(hpLeft.GetComponentsInChildren<Image>()[hpLeft.GetComponentsInChildren<Image>().Length - 1]);
            return false;
        }
        else
        {
            if(hpLeft.GetComponentsInChildren<Image>().Length > 0)
            {
                Destroy(hpLeft.GetComponentsInChildren<Image>()[hpLeft.GetComponentsInChildren<Image>().Length - 1]);

            }
            return true;

        }

    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 newPosition = hull.transform.localPosition;
        newPosition.x = Mathf.Sin(Time.time * speed) * amount + newPosition.x;
        hull.transform.localPosition = newPosition;


        if (hitDamageTime > Time.time)
            {
            Debug.Log("taking damage!");
//                Vector3 newPosition = hull.transform.position;
                newPosition.x = Mathf.Sin(Time.time * speed) * amount;
                hull.transform.position = newPosition;
            }
        else
        {
            hull.transform.position = originalPosition;
        }
        */
    }


}
