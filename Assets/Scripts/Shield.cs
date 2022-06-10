using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int defense = 10;
    private int power = 0;
    private Color startColor;
    private bool usedShieldBefore = false;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    public void Setup()
    {
        power = defense;
        if (!usedShieldBefore)
        {
            startColor = GetComponent<SpriteRenderer>().color;
            usedShieldBefore = true;
        }
        GetComponent<SpriteRenderer>().color = startColor;
        
    }

    public bool Hit(int amount)
    {
        power -= amount;
        Color currentColor = GetComponent<SpriteRenderer>().color;
        currentColor.a = currentColor.a * .5f;
        GetComponent<SpriteRenderer>().color = currentColor;
        if(power <= 0)
        {
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 offset = new Vector3();
        offset.x = Mathf.PerlinNoise(NoiseIndex.x, 0) - 0.5F;
        offset.y = Mathf.PerlinNoise(NoiseIndex.y, 0) - 0.5F;
        offset.z = Mathf.PerlinNoise(NoiseIndex.z, 0) - 0.5F;

        offset.Scale(WobbleAmount);
        // Set the position to the BasePosition plus the offset
        transform.position = BasePosition + offset;

        // Increment the NoiseIndex so that we get a new Noise value next time.
        NoiseIndex += WobbleSpeed * Time.deltaTime;
    */
    }
}
