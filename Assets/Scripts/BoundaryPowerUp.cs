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
    private Transform lastKnownBorderEnd;
    

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
        lastKnownBorderEnd = border.transform;
        SetBorderStrength(strength-1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetBorderStrength(int amount)
    {
        strength = amount;
        border.material.SetFloat("_Thickness", strength);
    }

    public void AddBorders(int amount)
    {
        strength += amount;
        border.material.SetFloat("_Thickness", strength);
    }

    public bool Hit()
    {
//        hits++;
        strength--;
        if (strength <= 0)
        {
            return true;
        }

        SetBorderStrength(strength);         
        return false;
        
    }
}
