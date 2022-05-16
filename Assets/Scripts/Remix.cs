using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remix : MonoBehaviour
{
    public ProceduralLevel level;
    public SpriteRenderer primary;
    public SpriteRenderer secondary;
    public SpriteRenderer energy;
    public SpriteRenderer ship;
    public SpriteRenderer hazard;
    private Renderer rend;    
    // Start is called before the first frame update
    void Start()
    {
        SetColors();        
    }

    public void SetColors()
    {
        //        rend = GetComponent<SpriteRenderer>().GetComponent<Renderer>();
        //       rend.material.SetFloat("_GlitchInterval", Random.Range(0f,.4f));
        if (!level)
        {
            level = GetComponentInParent<ProceduralLevel>();
        }

        if (primary)
        {
            primary.color = level.primaryColor;
        }

        if (secondary)
        {
            secondary.color = level.secondaryColor;

        }
        if (energy)
        {
            energy.color = level.energyColor;

        }

        if (ship)
        {
            ship.color = level.shipColor;

        }
        if (hazard)
        {
            if(gameObject.tag == "Avoid")
            {
                hazard.color = level.hazardColor;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
