using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public float minRange = 20f;
    public float maxRange = 340f;
    public float pulseSpeed = 1f;
    private float pulseDirection = 1;
    private float currentRange;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetScale()
    {
        currentRange = GetComponent<Light>().range;
        minRange = transform.parent.localScale.x * 2f;
        maxRange = transform.parent.localScale.x * 3f;
        Debug.Log("Using Scale of " + transform.parent.name);

    }

    // Update is called once per frame
    void Update()
    {
        currentRange += pulseSpeed * pulseDirection;
        GetComponent<Light>().intensity = Random.Range(1f, 1.5f);
        GetComponent<Light>().range = currentRange;
        if(currentRange > maxRange || currentRange < minRange)
        {
            pulseDirection *= -1;
        }
    }
}
