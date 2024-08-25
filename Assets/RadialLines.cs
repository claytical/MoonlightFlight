using UnityEngine;

public class RadialLines : MonoBehaviour
{
    public int numberOfRays = 10;            // Number of rays to draw
    public float maxRayLength = 5f;          // Maximum length of the rays
    public float expansionRate = 1f;         // Rate at which the rays expand
    public Color rayColor = Color.white;     // Color of the rays

    public AnimationCurve widthCurve;        // Curve for varying width along the ray's length
    public float maxWidth = 0.1f;            // Maximum width of the line

    private LineRenderer[] lineRenderers;
    private float currentLength = 0f;

    void Start()
    {
        // Create line renderers for each ray
        lineRenderers = new LineRenderer[numberOfRays];

        for (int i = 0; i < numberOfRays; i++)
        {
            GameObject lineObj = new GameObject("Ray" + i);
            lineObj.transform.parent = transform;

            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = rayColor;
            lineRenderer.endColor = rayColor;
            lineRenderer.positionCount = 2;  // Each line has a start and end point
            lineRenderer.widthCurve = widthCurve;  // Assign the width curve
            lineRenderer.widthMultiplier = maxWidth;  // Set the maximum width
            lineRenderers[i] = lineRenderer;
        }
    }

    void Update()
    {
        // Increment the length of the rays
        currentLength += expansionRate * Time.deltaTime;

        // Clamp the length to the maximum length
        currentLength = Mathf.Clamp(currentLength, 0f, maxRayLength);

        // Update each line renderer
        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfRays;
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

            lineRenderers[i].SetPosition(0, transform.position); // Start position (center)
            lineRenderers[i].SetPosition(1, transform.position + direction * currentLength); // End position (growing outward)
        }
    }
}
