using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutateOnCollision : MonoBehaviour
{

    public GameObject mutation;
    public void Mutate(GameObject mutation)
    {
        GameObject go = Instantiate(mutation, transform.position, transform.rotation, transform.parent);
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        Mutate(mutation);

    }


}
