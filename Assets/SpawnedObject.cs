using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject : MonoBehaviour
{

    private float lifeTime = 9999;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= lifeTime && lifeTime != 9999)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetLifeTime(float life)
    {
        lifeTime = Time.time + life;
    }
}
