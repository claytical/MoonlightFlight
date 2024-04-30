using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public int damage = 0;
    private Transform[] edges;
    // Start is called before the first frame update
    void Start()
    {
        
        edges = GetComponentsInChildren<Transform>();
            for(int i = 0; i < edges.Length; i++)
            {
                Hazard h = edges[i].gameObject.AddComponent<Hazard>();
                h.damage = damage;
                h.hazardType = Hazard.EnemyType.BOUNDARY;
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
