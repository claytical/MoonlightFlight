﻿using UnityEngine;
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
        /*
		if (!halfPipeMotion) {
			if (points.Count > 0) {
				GameObject g = new GameObject ();
				g.transform.position = gameObject.transform.localPosition;
				points.Add (g.transform);
			}
		}
        */
	}
	
	// Update is called once per frame
	void Update () {
		if (points.Count > 1) {
			transform.position = Vector2.MoveTowards (transform.position, points [currentPoint].position, speed * Time.deltaTime);
            /* 
            if (halfPipeMotion) {

				if (transform.position == points [currentPoint].position) {
					if (currentPoint == points.Count - 1) {
						direction = -1;
					}
					if (currentPoint == 0) {
						direction = 1;
					}
					currentPoint += direction;
				}


			} else {
			*/
            if (Vector2.Distance(transform.position, points[currentPoint].position) < 0.001f)
            {
        
                currentPoint++;
                if (currentPoint == points.Count)
                {
                    currentPoint = 0;
                }
            }

/*
            if (transform.position == points [currentPoint].position) {
					currentPoint++;
					if (currentPoint == points.Count) {
						currentPoint = 0;
					}
				}

		//	}
  */      
		}
	}
}
