using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remix : MonoBehaviour
{
    public SpriteRenderer primary;
    public SpriteRenderer hazard;
   

    public SpriteRenderer border;
    public SpriteRenderer identifier;
    public SpriteRenderer subidentifier;
    public SpriteRenderer box;
    public SpriteRenderer energy;
    //    public SpriteRenderer ship;
    private ProceduralLevel level;
    private RemixManager remix;
    private Renderer rend;
    private Color originalIdentifierColor;
    // Start is called before the first frame update
    void Start()
    {
        remix = FindObjectOfType<RemixManager>();
        level = FindObjectOfType<ProceduralLevel>();
    }

    public Color GetHazardColor()
    {
        if(remix)
        {
            return remix.hazardColor;

        }
        else
        {
            return Color.red;
        }
    }

    public Color GetOriginalIdentifierColor()
    {
        return originalIdentifierColor;
    }
    public void SetColors()
    {
        if (!remix)
        {
            remix = GetComponentInParent<RemixManager>();
        }
        else
        {
            Debug.Log("No remix manager found...");
        }

        if(primary)
        {
            primary.color = remix.primaryColor;
        }
        
        if(hazard)
        {
            hazard.color = remix.hazardColor;
        }

        if(subidentifier)
        {
            subidentifier.color = remix.secondaryColor;
            
//            level.secondaryColor = subidentifier.color;
        }
        if(box)
        {
            box.color = remix.boxColor;
        }
        if (identifier)
        {

            if (GetComponent<Hazard>())
            {
                identifier.color = remix.hazardColor;
            }
            else {
                identifier.color = remix.secondaryColor;
                originalIdentifierColor = identifier.color;

            }
            if (GetComponent<SpawnsObjects>()) {
                if (GetComponent<SpawnsObjects>().NextSpawnedObject().GetComponent<Hazard>())
                {
                    identifier.color = remix.hazardColor;
                }

            }

        }

        if (energy)
        {
            energy.color = remix.energyColor;

        }

        

    }


}
