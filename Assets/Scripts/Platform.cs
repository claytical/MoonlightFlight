using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    public GameObject platform;
    public bool indestructable = true;
    public RigidbodyConstraints2D constraints;

    public float scaleSpeed = .1f;
    [Range(0.1f, 5f)]
    public float timeToAppear = .1f;
    private float gravity = 0;
    private bool finished = false;
    private float scaleDirection = 1;
    private bool scaling = false;
    private Vector3 originalScale;
    [Range(0.1f, 1f)]
    public float fadeInDuration = .5f;

    private SpriteRenderer spriteRenderer;
    private float startTime;
    private Color initialColor;
    private bool fadingIn = true;
    private bool hasSetColors = false;

    // Start is called before the first frame update
    void Start()
    {
        //this makes a shortcut to the game object's sprite renderer
        spriteRenderer = platform.GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
        {
            if (gameObject.GetComponentInChildren<SpriteRenderer>())
            {
                spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            }
        }
            originalScale = platform.transform.localScale;
        TurnOffCollision();
        timeToAppear = Random.Range(.1f, .4f) + Time.time;// Time.time + timeToAppear;        
    }
    public void SetColors(RemixManager remix)
    {
        if (GetComponent<Remix>())
        {
            if (platform.GetComponent<Hazard>())
            {
                initialColor = remix.hazardColor;
            }
            else
            {
                initialColor = remix.primaryColor;
            }
        }
        else
        {
            initialColor = spriteRenderer.color;
        }

        // Set the initial color to transparent
        Color transparentColor = initialColor;
        transparentColor.a = 0f;
        if (spriteRenderer)
        {
            spriteRenderer.color = transparentColor;
        }
        else if (gameObject.GetComponentInChildren<SpriteRenderer>())
        {
            spriteRenderer.color = transparentColor;

        }
        //set for fading
        startTime = Time.time;
        hasSetColors = true;

    }


    void Update()
    {
        if(Time.time >= timeToAppear)
        {
//            ParticleSystem.MainModule ps = GetComponentInChildren<ParticleSystem>().main;
//            ps.loop = false;
//            Destroy(GetComponentInChildren<ParticleSystem>().gameObject,1);
            ScalePlatformUp();
        }

        if(hasSetColors)
        {
            // Calculate the current time since the start of fading
            float elapsedTime = Time.time - startTime;

            // Calculate the interpolation factor
            float t = Mathf.Clamp01(elapsedTime / fadeInDuration);

            // Interpolate between transparent and the initial color
            Color currentColor = Color.Lerp(Color.clear, initialColor, t);

            // Apply the current color to the SpriteRenderer
            spriteRenderer.color = currentColor;

            if (Time.time >= timeToAppear)
            {
                if (fadingIn)
                {
                    if (currentColor.a >= initialColor.a)
                    {
                        currentColor.a = initialColor.a;
                        fadingIn = false;
                    }
                }

            }

        }

        if (scaling)
        {
            Scale();
        }
    }

    void ScalePlatformUp()
    {
        platform.SetActive(true);
        platform.transform.localScale = originalScale;
        scaling = true;
    }

    public void Scale()
    {
        Vector3 newScale = new Vector3();

        newScale.x = platform.transform.localScale.x + (scaleSpeed * scaleDirection);
        newScale.y = platform.transform.localScale.y + (scaleSpeed * scaleDirection);
        newScale.z = platform.transform.localScale.z + (scaleSpeed * scaleDirection);
        platform.transform.localScale = newScale;

        if (newScale.x <= 0)
        {
            scaleDirection *= -1;
        }
        if (newScale.x >= originalScale.x)
        { 
            scaleDirection *= -1;
            scaling = false;
            platform.transform.localScale = originalScale;
            
        }

    }

    public void ScaleUp()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, .1f);
    }

    public bool CheckScale()
    {
        return transform.localScale == originalScale;
    }

    public void TurnOffCollision()
    {
        if (GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = false;

        }
        if (GetComponent<CircleCollider2D>())
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }

        if (GetComponent<PolygonCollider2D>())
        {
            GetComponent<PolygonCollider2D>().enabled = false;
        }

    }

    public void TurnOnCollision()
    {
        if (GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = true;

        }

        if(GetComponent<CircleCollider2D>())
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }

        if (GetComponent<PolygonCollider2D>())
        {
            GetComponent<PolygonCollider2D>().enabled = true;
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        if(!indestructable)
        {
            if(GetComponentInChildren<Explode>()) {
                GetComponentInChildren<Explode>().UntilNextSet();
            }

            if (GetComponent<Explode>())
            {
                GetComponent<Explode>().UntilNextSet();
            } 
        }

    }
  
    public void SetConstraints()
    {
        GetComponent<Rigidbody2D>().constraints = constraints;
        GetComponent<Rigidbody2D>().gravityScale = gravity;
    }


    //ANIMATION TRIGGERS
    public void Disappear() {
            Destroy(this.gameObject);
    }

    public bool isFinished()
    {
        return finished;
    }
    public void Finished(bool collision)
    {
        //CALLED IN INITIAL ANIMATION
        finished = true;
        if(collision)
        {
            TurnOnCollision();
        }

    }

}
