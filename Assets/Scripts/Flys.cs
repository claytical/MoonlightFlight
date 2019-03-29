using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flys : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Free()
    {
        Fly[] flies = GetComponentsInChildren<Fly>();
        for (int i = 0; i < flies.Length; i++)
        {
            flies[i].free = true;
        }
    }
}
