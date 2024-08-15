using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Vehicle>())
        {
            coll.gameObject.GetComponent<Vehicle>().CollectEnergy();
            GetComponent<Explode>().Permanent();
        }

    }

    private void OnDestroy()
    {
        if (GetComponentInParent<ProceduralLevel>())
        {
            Debug.Log("Checking for all collectables...");
            if (GetComponentInParent<ProceduralLevel>().AllObjectsCollected())
            {
                Debug.Log("All Objects Collected! Building Next Set");
                GetComponentInParent<ProceduralLevel>().RemovePlatforms();
                GetComponentInParent<ProceduralLevel>().BuildNextSet();
            }
            else
            {
                Debug.Log("Objects remain to be collected.");

            }
        }

    }

}