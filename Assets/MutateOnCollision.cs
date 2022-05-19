using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutateOnCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll.gameObject.name + " collding with " + this.name);
        if (coll.gameObject.tag == "Bumpable")
        {

            if (coll.gameObject.GetComponent<Breakable>())
            {
                Debug.Log("THIS BUMPABLE IS BREAKABLE!");
                //CAN IT USE OTHER OBJECTS TO BREAK IT?
                GetComponentInParent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<Breakable>().hit);
            }

            if (coll.gameObject.GetComponent<Platform>())
            {
                Debug.Log(coll.gameObject.name + " collding with " + this.name + " it's a platform too.");

                if (coll.gameObject.GetComponent<Platform>().canBePushed)
                {
                    Debug.Log(coll.gameObject.name + " pushing " + this.name);

                    GetComponent<Rigidbody2D>().isKinematic = false;
                    GetComponent<Platform>().SetConstraints();
                }

                if (gameObject.GetComponent<Platform>().canTurnHazardous)
                {
                    Debug.Log(gameObject.name + " turning hazardous " + this.name);

                    gameObject.GetComponent<Platform>().TurnHazardous();
                }

                if (coll.gameObject.GetComponent<Platform>().canSpawnNewObjects)
                {
                    Debug.Log(coll.gameObject.name + " spawning for " + this.name);

                    coll.gameObject.GetComponent<Platform>().SpawnObject();
                }
            }
            //PLAY COLLISION SOUND EFFECT
            GetComponentInParent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<CollisionSound>().soundFx[0], .5f);

        }

        if (coll.gameObject.tag == ("Power Up"))
        {

            //CAN IT COLLECT POWER UPS?
            if (coll.gameObject.GetComponent<PowerUp>())
            {
            }

        }


        if (coll.gameObject.tag == "Avoid")
        {
            //SHOULD IT DESTROY BUMPABLE OBJECTS?
        }

        gameObject.GetComponentInParent<AudioSource>().Play();
    }


}
