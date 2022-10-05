using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDirection : MonoBehaviour
{
    public Vector2 direction;
    public bool moveAutomatically;
    public bool reverseDirection = false;
    public Transform[] movementPoints;
    public RigidbodyConstraints2D movementConstraints;
    // Start is called before the first frame update
    void Start()
    {
        if(movementPoints.Length <= 0)
        {
            //moves on its own, needs constraints
            for(int i = 0; i < GetComponentsInChildren<Platform>().Length; i++)
            {
                GetComponentsInChildren<Platform>()[i].constraints = movementConstraints;
                GetComponentsInChildren<Platform>()[i].SetConstraints();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
            if (moveAutomatically)
            {
                if (GetComponent<Rigidbody2D>())
                {
                    GetComponent<Rigidbody2D>().AddForce(direction * 10f, ForceMode2D.Impulse);

                }
                else
                {
                    if (transform.GetComponentsInChildren<Rigidbody2D>().Length > 0)
                    {
                        for (int i = 0; i < transform.GetComponentsInChildren<Rigidbody2D>().Length; i++)
                        {
                            Rigidbody2D platform = transform.GetComponentsInChildren<Rigidbody2D>()[i];
                            platform.AddForce(direction * 10f, ForceMode2D.Impulse);
                        }

                    }
                }

            }
//        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (reverseDirection)
            {
                direction *= -1;
            }
            GetComponent<Rigidbody2D>().AddForce(direction * 10f, ForceMode2D.Force);
        }

    }

}
