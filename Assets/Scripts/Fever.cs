using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fever : MonoBehaviour {
    public GameObject feverBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void increaseFever()
    {
        GameObject bar = Instantiate(feverBar, transform);

    }
}
