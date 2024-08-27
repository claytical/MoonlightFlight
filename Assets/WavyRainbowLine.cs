using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WavyRainbowLine : MonoBehaviour
{
    public Color startColor = Color.red;  // Starting color of the rainbow
    public int numberOfLines = 7;  // Number of lines in the rainbow
    public int numberOfPoints = 100;  // Number of points on the wavy line
    public float amplitude = 0.5f;  // Amplitude of the wave
    public float wavelength = 2f;  // Wavelength of the wave
    public float verticalFraction = 0.33f;  // Fraction of the screen height the wave will take up
    public float lineThickness = 0.2f;  // Thickness of the line
    public float lineLifetime = 5f;  // Time in seconds before the lines disappear
    public float fadeDuration = 1f;  // Duration of the fade-out effect
    public float offScreenExtension = 0.1f;  // Extend the line beyond the screen by 10% of the screen width

    private LineRenderer lineRenderer;
    private float screenWidthInUnits;
    private float screenHeightInUnits;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        screenWidthInUnits = Camera.main.orthographicSize * Camera.main.aspect * 2f;
        screenHeightInUnits = Camera.main.orthographicSize * 2f;

        lineRenderer.positionCount = numberOfPoints;
        lineRenderer.widthMultiplier = 0.1f;

        Color[] rainbowColors = GenerateRainbowColors(startColor, numberOfLines);

        for (int i = 0; i < numberOfLines; i++)
        {
            StartCoroutine(DrawWavyLine(rainbowColors[i], i));
        }
    }

    IEnumerator DrawWavyLine(Color lineColor, int lineIndex)
    {
        LineRenderer lr = CreateLineRenderer(lineColor);
        Vector3[] positions = new Vector3[numberOfPoints];

        // Adjust the vertical offset based on the line index and thickness
        float yOffset = lineIndex * (lineThickness * 0.9f);  // 90% of lineThickness to reduce black space

        // Adjust the X start and end positions to extend off-screen
        float startX = -screenWidthInUnits * (0.5f + offScreenExtension);  // Extend start position to the left
        float endX = screenWidthInUnits * (0.5f + offScreenExtension);  // Extend end position to the right

        // Calculate the positions of the points along the wave
        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = (float)i / (numberOfPoints - 1);
            float x = Mathf.Lerp(startX, endX, t);  // Start and end off-screen
            float y = Mathf.Sin(t * wavelength * Mathf.PI * 2) * amplitude * screenHeightInUnits * verticalFraction + yOffset;
            positions[i] = new Vector3(x, y, 0);
            lr.SetPosition(i, positions[i]);
        }

        // Draw the line over time
        for (int i = 0; i < numberOfPoints; i++)
        {
            lr.positionCount = i + 1;
            lr.SetPosition(i, positions[i]);
            yield return new WaitForSeconds(0.01f);
        }

        // Wait for the line lifetime duration
        yield return new WaitForSeconds(lineLifetime);

        // Start fading out the line
        yield return StartCoroutine(FadeOutLine(lr));
    }

    LineRenderer CreateLineRenderer(Color color)
    {
        GameObject lineObject = new GameObject("RainbowLine");
        LineRenderer lr = lineObject.AddComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = lineThickness;
        lr.endWidth = lineThickness;
        lr.positionCount = numberOfPoints;
        lr.useWorldSpace = false;

        return lr;
    }

    Color[] GenerateRainbowColors(Color startColor, int numberOfColors)
    {
        Color[] colors = new Color[numberOfColors];
        float hue, saturation, value;

        Color.RGBToHSV(startColor, out hue, out saturation, out value);

        for (int i = 0; i < numberOfColors; i++)
        {
            float newHue = (hue + ((float)i / numberOfColors)) % 1f;
            colors[i] = Color.HSVToRGB(newHue, saturation, value);
        }

        return colors;
    }

    private IEnumerator FadeOutLine(LineRenderer lr)
    {
        float elapsedTime = 0f;
        Color startColor = lr.startColor;
        Color endColor = lr.endColor;

        // Ensure that the start and end colors are the same and both are fully opaque initially
        startColor.a = 1f;
        endColor.a = 1f;

        // Loop until the line is fully faded out
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            // Interpolate the alpha value towards 0 (fully transparent)
            Color newColor = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0), t);

            // Force alpha to zero if nearing the end of the fade
            if (elapsedTime >= fadeDuration - Time.deltaTime)
            {
                newColor.a = 0f;
            }

            lr.startColor = newColor;
            lr.endColor = newColor;

            yield return null;
        }

        // Ensure the line is fully transparent
        lr.startColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        lr.endColor = new Color(endColor.r, endColor.g, endColor.b, 0);

        Debug.Log("Fade-out complete. Destroying line.");

        // Destroy the line object after the fade-out is complete
        Destroy(lr.gameObject);
    }

}
