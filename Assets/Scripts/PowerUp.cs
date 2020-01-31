using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [System.Serializable]
    public enum Reward
    {
        Shield,
        Boundary,
        PassThrough,
        SpeedUp,
        SlowDown
    };

    public Reward reward;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
