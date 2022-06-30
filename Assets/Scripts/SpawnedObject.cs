using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    public Collider2D parentCollider;
    private float lifeTime = 9999;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Collider2D>())
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

        // Update is called once per frame
        void Update()
        {



        if (Time.time >= lifeTime && lifeTime != 9999)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetLifeTime(float life)
    {
        lifeTime = Time.time + life;
    }
}
