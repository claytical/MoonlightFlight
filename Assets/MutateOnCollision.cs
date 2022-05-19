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
            }

            if (coll.gameObject.GetComponent<Platform>())
            {

                if (gameObject.GetComponent<Platform>().canBePushed)
                {
                    Debug.Log(gameObject.name + " pushing " + coll.gameObject.name);

                    GetComponent<Rigidbody2D>().isKinematic = false;
                    GetComponent<Platform>().SetConstraints();
                }

                if (gameObject.GetComponent<Platform>().canTurnHazardous)
                {
                    Debug.Log(gameObject.name + " turning hazardous " + this.name);

                    gameObject.GetComponent<Platform>().TurnHazardous();
                }

                if (gameObject.GetComponent<Platform>().canSpawnNewObjects)
                {
                    Debug.Log(coll.gameObject.name + " spawning for " + this.name);

                    gameObject.GetComponent<Platform>().SpawnObject();
                }
            }

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

    }


}
