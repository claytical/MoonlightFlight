using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flys : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GetComponentsInChildren<Fly>().Length == 0)
        {
            Destroy(gameObject);
        }
	}
    public void Free(GameObject ball)
    {
        Fly[] flies = GetComponentsInChildren<Fly>();
        for (int i = 0; i < flies.Length; i++)
        {
            flies[i].free = true;
            flies[i].timeFreed = Time.frameCount + 100;
            flies[i].ball = ball;
        }
        ball.GetComponent<Ball>().force += .01f;

    }
}
