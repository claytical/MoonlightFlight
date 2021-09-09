using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vanishing : MonoBehaviour
{
    public float timeBetweenVanishing;
    private GameObject[] sprites;
    private int currentIndex;
    private float nextVanishingTime;
    public bool sequential = true;

    // Start is called before the first frame update
    void Start()
    {
        nextVanishingTime = Time.time + timeBetweenVanishing;
        currentIndex = 0;
        SpriteRenderer[] bodies = GetComponentsInChildren<SpriteRenderer>();
        sprites = new GameObject[bodies.Length];
        for (int i = 0; i < bodies.Length; i++)
        {
            sprites[i] = bodies[i].gameObject;
            sprites[i].SetActive(false);
        }
    }

    // Update is called once per frame
    private void TurnOff()
    {
        for(int i = 0; i < sprites.Length; i++)
        {
            sprites[i].SetActive(false);
        }
    } 

    void Update()
    {
        if (Time.time >= nextVanishingTime)
        {
            nextVanishingTime = Time.time + timeBetweenVanishing;
            if (sequential)
            {
                TurnOff();
                sprites[currentIndex].SetActive(true);
                currentIndex++;
                if (currentIndex >= sprites.Length)
                {
                    currentIndex = 0;
                }
//                sprites[currentIndex].SetActive(false);
            }
            else
            {
                for(int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].SetActive(false);
                }
                currentIndex = Random.Range(0, sprites.Length);
                sprites[currentIndex].SetActive(true);
            }

        }

    }
}
