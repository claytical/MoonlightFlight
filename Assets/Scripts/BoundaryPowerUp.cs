using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryPowerUp : MonoBehaviour

{
    public GameObject topCollider;
    public GameObject bottomCollider;
    public GameObject leftCollider;
    public GameObject rightCollider;
    public SpriteRenderer border;
    public GameObject smallerBorder;
    public int strength = 3;
    private int hits; //count the amount of hits on the bumpers

    //    private float timer; //allow the powerup to stay until it times out

    // Start is called before the first frame update

    public void SetCollisionBorders()
    {
        Vector2 minimum = new Vector3(border.bounds.min.x, border.bounds.min.y);
        Vector3 maximum = new Vector3(border.bounds.max.x, border.bounds.max.y);

        leftCollider.transform.position = new Vector2(minimum.x, 0);
        rightCollider.transform.position = new Vector2(maximum.x, 0);
        bottomCollider.transform.position = new Vector2(0, minimum.y);
        topCollider.transform.position = new Vector2(0, maximum.y);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBorderStrength(int amount)
    {
        strength = amount;
/*        Color borderColor = border.color;
        borderColor.a = 1;
        border.color = borderColor;
 */
    }

    public void AddBorders(int amount)
    {
        strength += amount;
        for(int i = 0; i < amount; i++)
        {
            Transform borderParent = GetComponentsInChildren<Border>()[GetComponentsInChildren<Border>().Length - 1].transform;
            if (borderParent)
            {
                Instantiate(smallerBorder, borderParent);
            }
            else
            {
                Instantiate(smallerBorder, transform);

            }
        }
    }

    public bool Hit()
    {
//        hits++;
        strength--;
        Color borderColor = border.color;
        if(strength <= 1)
        {
            borderColor.a = 0;
        }
        else
        {
            borderColor.a -= .3f;

        }
        border.color = borderColor;
        if (strength <= 0)
        {
            return true;
        }
        return false;
        
    }
}
