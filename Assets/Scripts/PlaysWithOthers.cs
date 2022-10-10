using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rigidbody2DExt
{

    public static void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
    {
        var explosionDir = rb.position - explosionPosition;
        var explosionDistance = explosionDir.magnitude;

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
            explosionDir /= explosionDistance;
        else
        {
            // From Rigidbody.AddExplosionForce doc:
            // If you pass a non-zero value for the upwardsModifier parameter, the direction
            // will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir, mode);
    }
}
public class PlaysWithOthers : MonoBehaviour
{

    public bool canBeDestroyed = false;
    public bool canBreakOtherItems = false;
    public bool canSpawnItems = false;

    void OnCollisionEnter2D(Collision2D coll)
    {

        if (canBeDestroyed)
        {
            if(gameObject.GetComponent<Explode>())
            {
                Debug.Log("Adding lots of force!");
                Rigidbody2DExt.AddExplosionForce(coll.rigidbody, 1000f, this.transform.position, 20f);

                GetComponent<Explode>().Go();
            }
        }

        if (coll.gameObject.GetComponent<SpawnsObjects>())
        {
            if (coll.gameObject.GetComponent<SpawnsObjects>().collisionCausesSpawn)
            {
                if(canSpawnItems)
                {

                    coll.gameObject.GetComponent<SpawnsObjects>().SpawnObject();

                }

                else
                {
                    Debug.Log("No spawning...");

                }
            }
        }

        if (coll.gameObject.GetComponent<Breakable>())
        {
            if (canBreakOtherItems)
            {

            }
            else
            {
                Vector2 pos = transform.position;
                Vector2 dir = coll.contacts[0].point - pos;
                dir = -dir.normalized;
                // And finally we add force in the direction of dir and multiply it by force. 
                // This will push back the player
                GetComponent<Rigidbody2D>().AddForce(dir * 3f, ForceMode2D.Impulse);


            }
        }

        if(gameObject.GetComponentInParent<OneDirection>())
        {
            if (gameObject.GetComponent<Rigidbody2D>())
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (gameObject.GetComponentInParent<OneDirection>().reverseDirection)
                {
                    gameObject.GetComponentInParent<OneDirection>().direction.x *= -1;
                }
                Debug.Log("game object:" + gameObject.name);
                gameObject.GetComponent<Rigidbody2D>().AddForce(gameObject.GetComponentInParent<OneDirection>().direction * 100f, ForceMode2D.Force);
            }

        }

    }




}
