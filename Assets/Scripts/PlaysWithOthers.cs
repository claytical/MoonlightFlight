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
    public bool canSpawnItems = false;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(canBeDestroyed)
        {
            if (coll.gameObject.GetComponent<Hazard>())
            {

            }
        }

        //THE OBJECT COLLIDING EXPLODES

        if (coll.gameObject.GetComponent<Explode>())
        {
            //THIS OBJECT ALSO EXPLODES

            if (GetComponent<Explode>())
            {
                //THIS OBJECT IS HAZARDOUS

                if (GetComponent<Hazard>())
                {
                    //THE COLLIDING OBJECT IS ALSO HAZARDOUS
                    if (coll.gameObject.GetComponent<Hazard>())
                    {
                        //THE COLLIDING OBJECT HAS GREATER OR EQUAL DAMAGE
                        if (coll.gameObject.GetComponent<Hazard>().damage >= GetComponent<Hazard>().damage)
                        {
                            GetComponent<Explode>().UntilNextSet();
                        }
                        else
                        {
                            Debug.Log("OTHER OBJECT SHOULD ALSO HAVE SAME COLLISION ROUTINE");
                        }
                    }
                    else
                    {
                        //COLLIDING OBJECT IS NOT HAZARDOUS, BUT THIS ONE IS
                        //                        Rigidbody2DExt.AddExplosionForce(coll.rigidbody, 1000f, this.transform.position, 20f);
                        //                        coll.gameObject.GetComponent<Explode>().Go();
                        //if this is a platform, it should not explode
                        //if this is not a platform, it should explode
                        if (canBeDestroyed)
                        {
                            if (GetComponent<Explode>())
                            {
                                GetComponent<Explode>().UntilNextSet();
                            }
                            else
                            {
                                Destroy(this.gameObject);
                            }
                        }
                    }
                }

                else
                {

                }
            }

            if (coll.gameObject.GetComponent<SpawnsObjects>())
            {
                if (canSpawnItems)
                {
//                        coll.gameObject.GetComponent<SpawnsObjects>().SpawnObject();
                } 
            }
        }
    }
}
