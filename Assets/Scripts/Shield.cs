using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private int power = 0;
    private Color startColor;
    private bool usedShieldBefore = false;
    /*
    public Vector3 WobbleAmount = new Vector3(0.1F, 0.1F, 0.1F);
    public Vector3 WobbleSpeed = new Vector3(0.5F, 0.5F, 0.5F);
    private Transform tr;
    private Vector3 BasePosition;
    private Vector3 NoiseIndex = new Vector3();
    */
    // Start is called before the first frame update
    void Start()
    {
        /*
        BasePosition = tr.position;

        NoiseIndex.x = Random.value;
        NoiseIndex.y = Random.value;
        NoiseIndex.z = Random.value;
    */
    }

    public void Setup(int amount)
    {
        power = amount;
        if (!usedShieldBefore)
        {
            startColor = GetComponent<SpriteRenderer>().color;
            usedShieldBefore = true;
        }
        GetComponent<SpriteRenderer>().color = startColor;
        
    }

    public void Hit(int amount)
    {
        power -= amount;
        Color currentColor = GetComponent<SpriteRenderer>().color;
        currentColor.a = currentColor.a * .5f;
        GetComponent<SpriteRenderer>().color = currentColor;
        if(power <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
