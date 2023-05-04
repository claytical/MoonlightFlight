using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outpost : MonoBehaviour
{
    public GameObject[] inTheWay;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnEnable()
    {
        if (!(inTheWay is null))
        {
            for (int i = 0; i < inTheWay.Length; i++)
            {
                inTheWay[i].SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        //NULL on self destruct?
        if(!(inTheWay is null))
        {
            for (int i = 0; i < inTheWay.Length; i++)
            {
                inTheWay[i].SetActive(true);
            }

        }

    }

    private void OnDestroy()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
