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

        TurnOffCollision();
        timeToAppear = Random.Range(0.1f, 0.4f) + Time.time;
        breathingRate = Random.Range(0.125f, 0.25f); // Breathing rate adjusted for slower scaling
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
        StartCoroutine(BreathingEffect());

        // Turn collision back on after scaling up
        TurnOnCollision();
    }

    public void SetColors(RemixManager remix)
    {
        if (GetComponent<Remix>())
        {
            if (platform.GetComponent<Hazard>())
            {
                originalColor = remix.hazardColor;
            }
            else
            {
                originalColor = remix.primaryColor;
            }
        }
        else
        {
            originalColor = spriteRenderer.color;
        }

        // Set the initial color to transparent
        Color transparentColor = originalColor;
        transparentColor.a = 0f;
//        spriteRenderer.color = transparentColor;

        // Set for fading
        startTime = Time.time;
        hasSetColors = true;
    }

    void Update()
    {
        if (Time.time >= timeToAppear)
        {
            ScalePlatformUp();
        }

        if (hasSetColors)
        {
            FadeInColor();
        }
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
        }
    }

    void ScalePlatformUp()
    {
        platform.SetActive(true);
        StartCoroutine(ScaleUpCoroutine());
    }

    private IEnumerator BreathingEffect()
    {
        while (true)
        {
            // Calculate the scale factor using a sine wave
            float scaleFactor = 0.75f + Mathf.PingPong(Time.time * breathingRate, 0.25f); // Scale from 0.75 to 1
            platform.transform.localScale = originalScale * scaleFactor;

            // Adjust alpha based on scale factor
            float alphaFactor = Mathf.Lerp(0.75f, 1f, scaleFactor);
            Color newColor = spriteRenderer.color;
            newColor.a = originalColor.a * alphaFactor;
            spriteRenderer.color = newColor;

            yield return null;
        }
    }

    public void ResetState()
    {
        // Reset the platform's transform properties to their original local values
        platform.transform.localPosition = originalLocalPosition;
        platform.transform.localRotation = originalLocalRotation;

        // Ensure the original scale is correctly restored
        if (originalScale == Vector3.zero)
        {
            Debug.LogWarning("Original scale is zero. Ensuring scale is set correctly.");
            originalScale = new Vector3(1f, 1f, 1f); // Hard-coded scale if original scale is not set
        }


        // Reset the scale of the platform (not the SpriteRenderer itself)
        platform.transform.localScale = Vector3.zero;

        // Reset the color and alpha if there's a SpriteRenderer
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);  // Ensure alpha starts as fully transparent
        }

        TurnOffCollision();  // Disable colliders initially
        platform.SetActive(true);
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
    }

    public void TurnOffCollision()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void TurnOnCollision()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }
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
