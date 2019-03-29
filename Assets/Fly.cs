using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {
    float angle = 0;
    float speed = (2 * Mathf.PI) / 2; //2*PI in degress is 360, so you get 5 seconds to complete a circle
    float radius = .008f;
    public bool free = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (free)
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * 1.0f));
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * 5f);

        }
        else
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * 1.0f));
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Time.deltaTime, Time.deltaTime));

            angle += speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
            Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            position.x += Mathf.Cos(angle) * radius;
            position.y += Mathf.Sin(angle) * radius;
            transform.position = position;
        }
    }
}
