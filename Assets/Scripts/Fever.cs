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
        if(transform.childCount > 26)
        {
            AudioSource[] go = gameObject.GetComponentsInChildren<AudioSource>();
            if (go.Length > 26)
            {
                for (int i = 0; i < go.Length - 26; i++)
                {
                    if (go[i].gameObject.GetComponent<AudioSource>())
                    {
                        Destroy(go[i].gameObject);
                    }
                }
            }
        }
		
	}

    public void increaseFever(int amount)
    {
        AudioSource[] go = gameObject.GetComponentsInChildren<AudioSource>();
        if (go.Length + amount < 27)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject bar = Instantiate(feverBar, transform);
            }

        } else if(go.Length < 27)
        {
            int resizedAmount = 26 - go.Length;
            for (int i = 0; i < resizedAmount; i++)
            {
                GameObject bar = Instantiate(feverBar, transform);

            }
        }
    }
    public void resetFever()
    {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

    
    }
}
