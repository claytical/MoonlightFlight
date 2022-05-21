using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Go()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Instantiate(explosion, transform.position, Quaternion.identity);
        GetComponent<Animator>().SetTrigger("destroy");
        GetComponent<Animator>().SetTrigger("disappear");

    }
}
