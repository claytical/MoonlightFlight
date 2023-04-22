using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    float radius = 0;
 //   float distanceToStar = 0;
    float angle = 0;
    float speed;


    void Start()
    {
        speed = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.parent.transform.position, Vector3.up, speed * Time.deltaTime);
    }

    public void AssignStartingOrbitPosition()
    {
        radius = transform.position.x;

        Vector3 pos = transform.position;

        angle = Mathf.Deg2Rad * Random.Range(0, 360);// -Mathf.PI;// * 1.5f;

        pos.x = (radius * Mathf.Cos(angle)) + pos.x;
        pos.z = (radius * Mathf.Sin(angle)) + pos.z;
        transform.position = pos;

    }
}
