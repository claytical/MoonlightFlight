using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remix : MonoBehaviour
{
    private RemixManager remixManager;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetRemixManager(RemixManager rm)
    {
        remixManager = rm;
        SetColorBasedOnScript();

    }
    public void AdjustColors()
    {
        remixManager.AdjustColors(.01f, false);
    }

    private void SetColorBasedOnScript()
    {
        if (GetComponentInChildren<Collectable>())
        {
            spriteRenderer.color = remixManager.collectable;
        }
        else if (GetComponentInChildren<Hazard>())
        {
            spriteRenderer.color = remixManager.hazard;

        }
        else if (GetComponent<Platform>())
        {
            Platform platformScript = GetComponent<Platform>();

            if (platformScript.indestructable)
            {
                spriteRenderer.color = remixManager.platform;

            }
            else
            {
                if(spriteRenderer)
                {
                    spriteRenderer.color = remixManager.breakable;

                }

            }
        }
        else
        {
            Debug.LogWarning("No relevant script found on this GameObject to determine color.");
        }
    }
}
