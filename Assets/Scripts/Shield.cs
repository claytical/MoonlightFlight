using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int defense = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool Hit(int amount)
    {
        defense -= amount;
        if(defense <= 0)
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
