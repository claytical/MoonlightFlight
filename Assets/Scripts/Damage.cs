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

    public GameObject hull;

//    private float speed = .01f;
//    private float amount = .01f;

//    private float hitDamageTime;
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

    public bool TakeDamage(int amount)
    {
        if(hull.GetComponent<Animator>()) {
            if(!hull.GetComponent<Animator>().IsInTransition(0))
            {
                hull.GetComponent<Animator>().SetTrigger("hit");

            }
        }
        if (hpLeft.GetComponentsInChildren<Image>().Length >= amount)
        {
            for(int i = amount; i > 0; i--)
            {
                Destroy(hpLeft.GetComponentsInChildren<Image>()[hpLeft.GetComponentsInChildren<Image>().Length - 1].gameObject);

            }
            return false;
        }
        else
        {
            return true;

        }

    }

    // Update is called once per frame
    void Update()
    {
    }


}
