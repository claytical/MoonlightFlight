using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    private bool activated = false;
    public Color startColor;
    // Start is called before the first frame update
    void Start()
    {
        startColor = GetComponent<SpriteRenderer>().color;
    }

    public void Activate()
    {
        activated = true;
        Color activeColor = startColor;
        activeColor.a = 100f;
        GetComponent<SpriteRenderer>().color = activeColor;
    }

    public void Deactivate()
    {
        activated = false;
        GetComponent<SpriteRenderer>().color = startColor;
    }

    public bool isActive()
    {
        return activated;
    }
}
