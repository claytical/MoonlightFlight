using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Moving : MonoBehaviour {
	//public bool moves = false;
	public float speed;
	public List<Transform> points;
	public Transform transformToMove;
	private int currentPoint = 0;
	private float originalRotation = 0;
	// Use this for initialization
	void Start () {
		GetOriginalRotation();
	}

	void GetOriginalRotation()
    {
		if (transformToMove.position.x < points[currentPoint].position.x)
		{
			originalRotation = 90;

		}
		if (transformToMove.position.x > points[currentPoint].position.x)
		{
			originalRotation = -90;
		}
		if (transformToMove.position.y < points[currentPoint].position.y)
        {
			originalRotation = 180;
        }
		if(transformToMove.position.y > points[currentPoint].position.y)
        {
			originalRotation = 0;
        }

		//transformToMove.Rotate(new Vector3(0, 0, originalRotation));

	}
	void SetMovementDirection()
    {
		if (transformToMove.position.x < points[currentPoint].position.x)
		{
			//moving left
//			transformToMove.Rotate(new Vector3(0, 0, 180));

		}
		if (transformToMove.position.x > points[currentPoint].position.x)
		{
			//moving right
//			transformToMove.Rotate(new Vector3(0, 0, -180));

		}
	}

	public Vector2 GetCurrentDirection()
    {
		float x = transformToMove.position.x - points[currentPoint].position.x;
		float y = transformToMove.position.y - points[currentPoint].position.y;
		return new Vector2(x, y);
    }
	
	// Update is called once per frame
	void Update () {
		if (points.Count > 1) {
			if(transformToMove)
            {
				transformToMove.position = Vector2.MoveTowards(transformToMove.position, points[currentPoint].position, speed * Time.deltaTime);
				if (Vector2.Distance(transformToMove.position, points[currentPoint].position) < 0.001f)
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
}
