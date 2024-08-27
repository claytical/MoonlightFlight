using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log($"Collision detected between {gameObject.name} and {coll.gameObject.name}");

        if (coll.gameObject.GetComponent<Vehicle>())
        {
            coll.gameObject.GetComponent<Vehicle>().CollectEnergy(amount);
            GetComponent<Explode>().Permanent();  // Destroy the collectable with an effect
        }
    }

    private void OnDestroy()
    {
        Debug.Log("I am being destroyed. " + gameObject.name);
        if(transform.parent.GetComponentInParent<SetInfo>() == null || transform.parent.parent.gameObject == null || transform.parent.parent.gameObject.Equals(null))
        {

        }
        else
        {
            ProceduralLevel proceduralLevel = transform.parent.GetComponentInParent<SetInfo>().proceduralLevel;
            if (proceduralLevel)
            {
                if (proceduralLevel.AllObjectsCollected())
                {
                    proceduralLevel.RemovePlatforms(); // Remove current platforms
                    proceduralLevel.BuildNextSet(); // Build the next set
                    proceduralLevel.NextPlane(transform);
                }
            }

        }
    }
}
