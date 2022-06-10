using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Moving : MonoBehaviour {
	//public bool moves = false;
	public float speed;
	public List<Transform> points;
	private int currentPoint = 0;
	private float originalRotation = 0;
	// Use this for initialization
	void Start () {
		GetOriginalRotation();
	}

	void GetOriginalRotation()
    {
		if (transform.position.x < points[currentPoint].position.x)
		{
			originalRotation = 90;

		}
		if (transform.position.x > points[currentPoint].position.x)
		{
			originalRotation = -90;
		}
		if (transform.position.y < points[currentPoint].position.y)
        {
			originalRotation = 180;
        }
		if(transform.position.y > points[currentPoint].position.y)
        {
			originalRotation = 0;
        }

		transform.Rotate(new Vector3(0, 0, originalRotation));

	}
	void SetMovementDirection()
    {
		if (transform.position.x < points[currentPoint].position.x)
		{
			//moving left
			transform.Rotate(new Vector3(0, 0, 180));

		}
		if (transform.position.x > points[currentPoint].position.x)
		{
			//moving right
			transform.Rotate(new Vector3(0, 0, -180));

		}
	}

	
	// Update is called once per frame
	void Update () {
		if (points.Count > 1) {
			transform.position = Vector2.MoveTowards (transform.position, points [currentPoint].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, points[currentPoint].position) < 0.001f)
            {
                currentPoint++;
                if (currentPoint == points.Count)
                {
                    currentPoint = 0;
                }
				//				transform.right = points[currentPoint].position - transform.position;
				SetMovementDirection();
			}
		}
	}
}
