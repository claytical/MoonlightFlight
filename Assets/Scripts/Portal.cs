using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
	public List<Transform> points;
	// Use this for initialization
	void Start () {

		foreach (Transform child in transform)
		{
			points.Add(child);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
}
