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
    public float moveSpeed = 1f;  // Speed at which the line moves upwards

    private float screenWidthInUnits;
    private float screenHeightInUnits;

    void Start()
    {
        screenWidthInUnits = Camera.main.orthographicSize * Camera.main.aspect * 2f;
        screenHeightInUnits = Camera.main.orthographicSize * 4f;

        Color[] rainbowColors = GenerateRainbowColors(startColor, numberOfLines);
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift = .2f;
        Invoke("ResetGlitch", .2f);

        for (int i = 0; i < numberOfLines; i++)
        {
            StartCoroutine(DrawAndMoveWavyLine(rainbowColors[i], i));
        }
    }
    void ResetGlitch()
    {
        Debug.Log("Resetting Glitch");
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().scanLineJitter = 0f;
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift = 0f;
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = 0f;
    }


    IEnumerator DrawAndMoveWavyLine(Color lineColor, int lineIndex)
    {
        LineRenderer lr = CreateLineRenderer(lineColor);
        Vector3[] positions = new Vector3[numberOfPoints];

        // Adjust the vertical offset based on the line index and thickness
        float yOffset = -screenHeightInUnits / 2f - lineThickness * lineIndex;  // Start below the screen
        float xOffset = 1;

        // Calculate the positions of the points along the wave
        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = (float)i / (numberOfPoints - 1);
            float x = Mathf.Lerp((-screenWidthInUnits / 2f) -1, (screenWidthInUnits / 2f) + 1, t);  // Start and end off-screen
            float y = Mathf.Sin(t * wavelength * Mathf.PI * 2) * amplitude * screenHeightInUnits * verticalFraction + yOffset;
            positions[i] = new Vector3(x, y, 0);
            lr.SetPosition(i, positions[i]);
        }

        // Draw the line immediately
        lr.positionCount = numberOfPoints;
        lr.SetPositions(positions);

        // Wait for the line to finish drawing
        yield return new WaitForSeconds(0.1f);  // Adjust if necessary to match drawing speed

        // Move the line upwards until it's off the screen
        while (lr.transform.position.y < screenHeightInUnits)
        {
            lr.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;
        }

        // Destroy the line object after it has moved off the screen
//        Destroy(lr.gameObject);
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
}
