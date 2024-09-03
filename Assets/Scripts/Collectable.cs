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

            var explode = GetComponent<Explode>();
            if (explode != null)
            {
                explode.Permanent();  // Destroy the collectable with an effect
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log("I am being destroyed. " + gameObject.name);
        if (transform.parent == null || transform.parent.GetComponentInParent<SetInfo>() == null)
        {
            return;
        }
        else
        {
            ProceduralLevel proceduralLevel = transform.parent.GetComponentInParent<SetInfo>().proceduralLevel;
            if (proceduralLevel != null && proceduralLevel.AllObjectsCollected())
            {
                proceduralLevel.RemovePlatforms(); // Remove current platforms
                proceduralLevel.BuildNextSet(); // Build the next set
                proceduralLevel.NextPlane(transform);
            }
        }
    }
}
