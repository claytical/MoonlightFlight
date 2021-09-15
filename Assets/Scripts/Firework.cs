using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{

    public float timeUntilExplosion;
    public GameObject touchPoint;
    // Start is called before the first frame update
    void Start()
    {
        timeUntilExplosion = timeUntilExplosion + Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector2.Distance(transform.position, touchPoint.transform.position);

        Debug.Log("MY DISTANCE: " + dist);
        if(dist <= 1)
        {
            Destroy(this.gameObject);

        }
    }
        

    public void SetTouchPoint(GameObject go)
    {
        touchPoint = go;
    }
}
