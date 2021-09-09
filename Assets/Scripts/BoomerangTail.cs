using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangTail : MonoBehaviour
{

    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<SpriteRenderer>())
        {
            sprite = GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Color newColor = sprite.color;
        newColor.a = Mathf.Lerp(10,200, Mathf.Abs(Mathf.Sin(Time.frameCount)));
        sprite.color = newColor;
    }
}
