using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remix : MonoBehaviour
{
    private ProceduralLevel level;
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
        level = FindObjectOfType<ProceduralLevel>();

        SetColors();        
    }

    public Color GetHazardColor()
    {
        if(level)
        {
            return level.hazardColor;

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
        if (!level)
        {
            level = GetComponentInParent<ProceduralLevel>();
        }

        if (border)
        {
            if (GetComponent<Breakable>())
            {
                border.color = level.energyColor;
            }

            else
            {
                border.color = level.borderColor;

            }

        }
        if(subidentifier)
        {
            subidentifier.color = level.secondaryColor;
            
//            level.secondaryColor = subidentifier.color;
        }
        if(box)
        {
            box.color = level.boxColor;
        }
        if (identifier)
        {

            if (GetComponent<Hazard>())
            {
                identifier.color = level.hazardColor;
            }
            else {
                identifier.color = level.secondaryColor;
                originalIdentifierColor = identifier.color;

            }
            if (GetComponent<SpawnsObjects>()) {
                if (GetComponent<SpawnsObjects>().NextSpawnedObject().GetComponent<Hazard>())
                {
                    identifier.color = level.hazardColor;
                }

            }

        }

        if (energy)
        {
            energy.color = level.energyColor;

        }

        if (ship)
        {
            ship.color = level.shipColor;

        }

    }


}
