using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {
    public int numberOfDebris;
    public GameObject[] debris;

	// Use this for initialization
	void Start () {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3 (0,0,0));
        Vector3 origin = Camera.main.ViewportToScreenPoint(new Vector3(-7, 0, 0));
        float startX = Camera.main.transform.position.x - (Screen.width / 2);
        //+ (_prefabSize.x / 2) + (_prefabSize.x * x);
        float startY = Camera.main.transform.position.y - (Screen.height / 2);
        //+ (_prefabSize.y / 2) + (_prefabSize.y * y);

        for (int i = 0; i < numberOfDebris; i++)
        {

            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
            Vector3 position = new Vector3(origin.x, origin.y, 0);
            Instantiate(debris[Random.Range(0, debris.Length)], screenPosition, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
