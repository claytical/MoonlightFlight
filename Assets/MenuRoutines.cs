using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRoutines : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Close()
    {
        GetComponent<Animator>().SetTrigger("close");
    }

    public void FinishedClosing()
    {
        gameObject.SetActive(false);
    }
}
