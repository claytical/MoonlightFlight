using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    public GameObject spawner;
    private float lifeTime = 9999;
    // Start is called before the first frame update

        // Update is called once per frame
    void Update()
    {
        /*
        if (Time.time >= lifeTime && lifeTime != 9999)
        {
            if(GetComponent<Explode>())
            {
                GetComponent<Explode>().Permanent();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        */
    }

    public void OnDestroy()
    {
        Debug.Log("Starting Particles before I die.");
        spawner.GetComponent<SpawnsObjects>().StartParticles();
    }
    public void SetLifeTime(float life, GameObject go)
    {
        lifeTime = Time.time + life;
        spawner = go;
    }
}
