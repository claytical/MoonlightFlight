using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectParent : MonoBehaviour {

	// Use this for initialization
	void Start () {
        LevelSelector[] levels = GetComponentsInChildren<LevelSelector>();
		for(int i = 0; i < levels.Length; i++)
        {
//            levels[i].SetLocks();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
