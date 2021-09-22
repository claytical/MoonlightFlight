using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public float secondsToDisplay = 1f;
    private float timer;
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < Time.time)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetTimer()
    {
        timer = Time.time + secondsToDisplay;

    }
}
