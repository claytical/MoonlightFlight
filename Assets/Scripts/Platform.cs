using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private Vector3 originalScale;
    private Color originalColor;

    public GameObject platform;  // This should be the object with the SpriteRenderer
    public bool indestructable = true;  // The indestructable property is reintroduced
    public RigidbodyConstraints2D constraints;

    public float scaleSpeed = 0.1f;
    [Range(0.1f, 5f)]
    public float timeToAppear = 0.1f;

    [Range(0.1f, 1f)]
    public float fadeInDuration = 0.5f;

    private SpriteRenderer spriteRenderer;
    private float startTime;
    private bool fadingIn = true;
    private bool hasSetColors = false;
    private float breathingRate;
    private ProceduralLevel level;

    private Collider2D[] colliders;

    void Start()
    {
        // Hard-coding original scale to (1,1,1) if the original scale is zero
        originalLocalPosition = platform.transform.localPosition;
        originalLocalRotation = platform.transform.localRotation;
        originalScale = platform.transform.localScale == Vector3.zero ? new Vector3(1f, 1f, 1f) : platform.transform.localScale;

        spriteRenderer = platform.GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
        {
            spriteRenderer = platform.GetComponentInChildren<SpriteRenderer>();
        }

        originalColor = spriteRenderer.color;

        // Set the initial scale of the platform to zero (the parent object)
        platform.transform.localScale = Vector3.zero;
        InitializeColliders();
        if (!IsBeingDestroyed())
        {
            TurnOffCollision();
        }

        timeToAppear = Random.Range(0.1f, 0.4f) + Time.time;
        breathingRate = Random.Range(0.125f, 0.25f); // Breathing rate adjusted for slower scaling
    }
    private void InitializeColliders()
    {
        // Fetch all Collider2D components in this object and its children
        colliders = GetComponentsInChildren<Collider2D>(true);

        if (colliders == null || colliders.Length == 0)
        {
            Debug.LogWarning($"No colliders found on {gameObject.name} or its children.");
        }
        else
        {
            Debug.Log($"{colliders.Length} colliders found on {gameObject.name}.");
        }
    }

    public void AttachLevel(ProceduralLevel _level)
    {
        level = _level;
    }

    void Update()
    {
        if (Time.time >= timeToAppear)
        {
            ScalePlatformUp();
        }

        // Handle the breathing effect less frequently (e.g., every 5 frames)
        if (Time.frameCount % 5 == 0)
        {
            ApplyBreathingEffect();
        }
    }

    private void FixedUpdate()
    {
        if (hasSetColors)
        {
            FadeInColor();
        }
    }

    private void ApplyBreathingEffect()
    {
        // Calculate the scale factor using a sine wave
        float scaleFactor = 0.75f + Mathf.PingPong(Time.time * breathingRate, 0.25f); // Scale from 0.75 to 1
        platform.transform.localScale = originalScale * scaleFactor;

        // Adjust alpha based on scale factor
        float alphaFactor = Mathf.Lerp(0.75f, 1f, scaleFactor);
        Color newColor = spriteRenderer.color;
        newColor.a = originalColor.a * alphaFactor;
        spriteRenderer.color = newColor;
    }

    private IEnumerator ScaleUpCoroutine()
    {
        float time = 0;

        // Scale up from 0 to the original scale over time
        while (time < scaleSpeed)
        {
            time += Time.deltaTime;
            platform.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, time / scaleSpeed);
            yield return null;
        }

        // Ensure the platform reaches its original scale
        platform.transform.localScale = originalScale;

        // Start the breathing effect
        // ApplyBreathingEffect() is now called periodically in Update()
        TurnOnCollision();
    }

    public void SetColors(RemixManager remix)
    {
        // Set for fading
        startTime = Time.time;
        hasSetColors = true;
    }

    private void FadeInColor()
    {
        // Calculate the current time since the start of fading
        float elapsedTime = Time.time - startTime;

        // Calculate the interpolation factor
        float t = Mathf.Clamp01(elapsedTime / fadeInDuration);

        // Interpolate between transparent and the initial color
        Color currentColor = Color.Lerp(new Color(originalColor.r, originalColor.g, originalColor.b, 0f), originalColor, t);

        // Apply the current color to the SpriteRenderer
        spriteRenderer.color = currentColor;

        if (currentColor.a >= originalColor.a)
        {
            spriteRenderer.color = originalColor;
            fadingIn = false;
            hasSetColors = false;  // Stop further updates once fading is complete
        }
    }

    void ScalePlatformUp()
    {
        platform.SetActive(true);
        StartCoroutine(ScaleUpCoroutine());
    }

    public void ResetState()
    {
        // Reset the platform's transform properties to their original local values
        platform.transform.localPosition = originalLocalPosition;
        platform.transform.localRotation = originalLocalRotation;

        // Ensure the original scale is correctly restored
        if (originalScale == Vector3.zero)
        {
            originalScale = new Vector3(1f, 1f, 1f); // Hard-coded scale if original scale is not set
        }

        // Reset the scale of the platform (not the SpriteRenderer itself)
        platform.transform.localScale = Vector3.zero;

        // Reset the color and alpha if there's a SpriteRenderer
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);  // Ensure alpha starts as fully transparent
        }

        platform.SetActive(true);
//        TurnOffCollision();  // Disable colliders initially

    }

    public void MoveOffScreen(Vector3 offScreenPosition, float duration)
    {
        StartCoroutine(MoveOffScreenCoroutine(offScreenPosition, duration));
    }

    private IEnumerator MoveOffScreenCoroutine(Vector3 offScreenPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, offScreenPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = offScreenPosition;
        TurnOffCollision();
        gameObject.SetActive(false);  // Deactivate the platform after moving it off-screen

        if(level != null)
        {
//            level.OnPlatformDeactivated()
        }

    }

    public void TurnOffCollision()
    {
        if (IsBeingDestroyed()) return;

        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider != null)
                {
                    collider.enabled = false;
                }
                else
                {
                    Debug.LogWarning($"Collider is null on {gameObject.name}.");
                }
            }
        }
        else
        {
            Debug.LogError($"Colliders array is null on {gameObject.name}.");
        }
    }


    public void TurnOnCollision()
    {
        if (IsBeingDestroyed()) return;

        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider != null)
                {
                    collider.enabled = true;
                }
                else
                {
                    Debug.LogWarning($"Collider is null on {gameObject.name}.");
                }
            }
        }
        else
        {
            Debug.LogError($"Colliders array is null on {gameObject.name}.");
        }
    }

    private bool IsBeingDestroyed()
    {
        return this == null || gameObject == null || gameObject.Equals(null);
    }

    void OnDestroy()
    {
        // Clean up any references or stop any running coroutines if necessary
        StopAllCoroutines();
    }
    public void Finished(bool enableCollision)
    {
        // Optionally enable collision if specified
        if (enableCollision)
        {
            TurnOnCollision();
        }
    }
}
