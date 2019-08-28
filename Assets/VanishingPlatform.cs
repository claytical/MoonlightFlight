using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour {
    public float timeBetweenVanishing;
    private GameObject[] platforms;
    private int currentIndex;
    private float nextVanishingTime;
    // Use this for initialization
	void Start () {
        nextVanishingTime = Time.time + timeBetweenVanishing;
        currentIndex = 0;
        Rigidbody2D[] bodies = GetComponentsInChildren<Rigidbody2D>();
        platforms = new GameObject[bodies.Length];
        for (int i = 0; i < bodies.Length; i++)
        {
            platforms[i] = bodies[i].gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(Time.time >= nextVanishingTime)
        {
            Debug.Log("Vanishing #" +currentIndex);
            //            platforms[currentIndex].SetActive(true);
            nextVanishingTime = Time.time + timeBetweenVanishing;
            platforms[currentIndex].SetActive(true);
            currentIndex++;
            if (currentIndex >= platforms.Length)
            {
                currentIndex = 0;
            }
            platforms[currentIndex].SetActive(false);


        }

    }
}
