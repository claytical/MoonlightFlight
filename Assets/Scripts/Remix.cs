using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remix : MonoBehaviour
{
    public ProceduralLevel level;
    public RemixManager remix;
    public SpriteRenderer border;
    public SpriteRenderer identifier;
    public SpriteRenderer subidentifier;
    public SpriteRenderer box;
    public SpriteRenderer energy;
    public SpriteRenderer ship;
    private Renderer rend;
    private Color originalIdentifierColor;
    // Start is called before the first frame update
    void Start()
    {
        remix = FindObjectOfType<RemixManager>();
//        level = FindObjectOfType<ProceduralLevel>();

        SetColors();        
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

        if (border)
        {
            if (GetComponent<Breakable>())
            {
                border.color = remix.energyColor;
            }

            else
            {
                border.color = remix.borderColor;

            }

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

        if (ship)
        {
            ship.color = remix.shipColor;

        }

    }


}
