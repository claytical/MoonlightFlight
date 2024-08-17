using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Vehicle>())
        {
            coll.gameObject.GetComponent<Vehicle>().CollectEnergy(amount);
            GetComponent<Explode>().Permanent();  // Destroy the collectable with an effect
        }
    }

    private void OnDestroy()
    {
        ProceduralLevel proceduralLevel = transform.parent.GetComponentInParent<SetInfo>().proceduralLevel;
        if (proceduralLevel)
        {
            if (proceduralLevel.AllObjectsCollected())
            {
                proceduralLevel.RemovePlatforms(); // Remove current platforms
                proceduralLevel.BuildNextSet(); // Build the next set
            }
            else
            {
            }
        }
    }
}
