using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirection : MonoBehaviour
{
    public Vector2 direction;
    public bool moveAutomatically; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(moveAutomatically)
        {
            GetComponent<Rigidbody2D>().AddForce(direction * 10f, ForceMode2D.Impulse);
        }
//        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(direction * 10f,ForceMode2D.Force);

    }

}
