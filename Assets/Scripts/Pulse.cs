using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    // Start is called before the first frame update

    public float scalingAmount;
    public float speed;
    private Vector3 originalScale;
    private Vector3 pulseScale;

    void Start()
    {
        originalScale = transform.localScale;
        pulseScale = originalScale * scalingAmount;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(originalScale, pulseScale, Mathf.Sin(Time.time * speed));
    }

}


