using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaysWithOthers : MonoBehaviour
{

    public bool canBeDestroyed = false;
    public bool canBreakOtherItems = false;
    public bool canSpawnItems = false;

    void OnCollisionEnter2D(Collision2D coll)
    {

        if (canBeDestroyed)
        {
            if (coll.gameObject.GetComponent<Hazard>())
            {
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
