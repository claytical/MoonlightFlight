using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Moving : MonoBehaviour {
	//public bool moves = false;
	public float speed;
	public List<Transform> points;
	public bool halfPipeMotion;
	public int currentPoint = 0;
	private int direction = 1;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (points.Count > 1) {
            Debug.Log("Moving Towards Point");
			transform.position = Vector2.MoveTowards (transform.position, points [currentPoint].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, points[currentPoint].position) < 0.001f)
            {
                Debug.Log("Hit Destination");

                currentPoint++;
                if (currentPoint == points.Count)
                {
                    Debug.Log("Returning");
                    currentPoint = 0;
                }
            }
		}
	}
}
