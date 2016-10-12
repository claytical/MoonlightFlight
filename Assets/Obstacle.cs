using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	public bool moves = false;
	public float min=2f;
	public float max=3f;

	// Use this for initialization
	void Start () {
		min=transform.position.x;
		max=transform.position.x+3;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(moves) {
			transform.position =new Vector3(Mathf.PingPong(Time.time*2,max-min)+min, transform.position.y, transform.position.z);	
		}
	}
}
