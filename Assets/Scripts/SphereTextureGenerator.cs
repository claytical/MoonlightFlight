using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    public int textureWidth = 512; // Width of the generated texture
    public int textureHeight = 512; // Height of the generated texture
    public Color color1 = Color.white; // First color used in textures
    public Color color2 = Color.black; // Second color used in textures
    public int TextureType = 0;
    private Texture2D generatedTexture;
    public int numSquares = 10; // Number of squares
    public float minSize = 20f; // Minimum size of squares
    public float maxSize = 100f; // Maximum size of squares
    public int numHoles = 50; // Number of holes in the cheese
    public float minHoleRadius = 10f; // Minimum radius of holes
    public float maxHoleRadius = 40f; // Maximum radius of holes
    public float lineThickness = 5f; // Thickness of the line
    public int numSteps = 1000; // Number of steps for the line to wander

    void Start()
    {
        switch(TextureType) {
            case 0:
                // Generate connected spiral texture
                GenerateConnectedSpiralTexture();

                break;
            case 1:
                // Generate checkerboard texture
                GenerateCheckerboardTexture();

                break;
            case 2:
                // Generate random noise texture
                GenerateRandomNoiseTexture();
                break;
            case 3:
                GenerateRandomSquaresTexture();
                break;
            case 4:
                GenerateSwissCheeseTexture();
                break;
            case 5:
                GenerateWanderingLineTexture();
                break;
        }


    }

    void GenerateConnectedSpiralTexture()
    {
        // Your existing connected spiral texture generation code
    }

    void GenerateCheckerboardTexture()
    {
        generatedTexture = new Texture2D(textureWidth, textureHeight);

        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                // Calculate checkerboard pattern based on pixel position
                Color pixelColor = ((x + y) % 2 == 0) ? color1 : color2;
                generatedTexture.SetPixel(x, y, pixelColor);
            }
        }

        generatedTexture.Apply();
        GetComponent<Renderer>().material.mainTexture = generatedTexture;
    }

    void GenerateRandomNoiseTexture()
    {
        generatedTexture = new Texture2D(textureWidth, textureHeight);

        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                // Generate random noise color for each pixel
                Color pixelColor = new Color(Random.value, Random.value, Random.value);
                generatedTexture.SetPixel(x, y, pixelColor);
            }
        }

        generatedTexture.Apply();
        GetComponent<Renderer>().material.mainTexture = generatedTexture;
    }

    void GenerateRandomSquaresTexture()
    {
        // Create a new texture
        generatedTexture = new Texture2D(textureWidth, textureHeight);

        // Loop through each square
        for (int i = 0; i < numSquares; i++)
        {
            // Generate random position and size for the square
            float squareX = Random.Range(0, textureWidth - maxSize);
            float squareY = Random.Range(0, textureHeight - maxSize);
            float squareSize = Random.Range(minSize, maxSize);

            // Loop through each pixel within the square's bounding box
            for (int y = (int)squareY; y < Mathf.Min(textureHeight, squareY + squareSize); y++)
            {
                for (int x = (int)squareX; x < Mathf.Min(textureWidth, squareX + squareSize); x++)
                {
                    // Set the color to the square color
                    generatedTexture.SetPixel(x, y, color1);
                }
            }
        }

        // Apply changes to the texture
        generatedTexture.Apply();

        // Assign the generated texture to a material
        GetComponent<Renderer>().material.mainTexture = generatedTexture;
    }
    void GenerateSwissCheeseTexture()
    {
        // Create a new texture
        generatedTexture = new Texture2D(textureWidth, textureHeight);

        // Loop through each pixel of the texture
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                // Set default pixel color to cheese color
                Color pixelColor = color1;

                // Loop through each hole
                for (int i = 0; i < numHoles; i++)
                {
                    // Generate random position for the hole
                    Vector2 holePosition = new Vector2(Random.Range(0, textureWidth), Random.Range(0, textureHeight));

                    // Calculate distance from current pixel to the hole position
                    float distanceToHole = Vector2.Distance(new Vector2(x, y), holePosition);

                    // Calculate radius of the hole
                    float holeRadius = Random.Range(minHoleRadius, maxHoleRadius);

                    // If the pixel is within the hole radius, set its color to the hole color
                    if (distanceToHole < holeRadius)
                    {
                        pixelColor = color2;
                        break; // Break the loop if the pixel is inside a hole
                    }
                }

                // Set the color of the current pixel in the texture
                generatedTexture.SetPixel(x, y, pixelColor);
            }
        }

        // Apply changes to the texture
        generatedTexture.Apply();

        // Assign the generated texture to a material
        GetComponent<Renderer>().material.mainTexture = generatedTexture;
    }

    void GenerateWanderingLineTexture()
    {
        // Create a new texture
        generatedTexture = new Texture2D(textureWidth, textureHeight);

        // Initialize position and direction of the line
        Vector3 currentPosition = Random.onUnitSphere;
        Vector3 currentDirection = Random.onUnitSphere;

        // Loop through each step of the line
        for (int i = 0; i < numSteps; i++)
        {
            // Calculate next position of the line
            Vector3 nextPosition = currentPosition + currentDirection * lineThickness * Random.Range(0.8f, 1.2f);

            // Convert positions to UV coordinates
            Vector2 currentUV = GetUVFromSpherePosition(currentPosition);
            Vector2 nextUV = GetUVFromSpherePosition(nextPosition);

            // Draw line segment on the texture
            DrawLineOnTexture(currentUV, nextUV, color1);

            // Update current position to next position
            currentPosition = nextPosition;

            // Randomly adjust direction of the line
            currentDirection = Quaternion.Euler(Random.Range(-30f, 30f), Random.Range(-30f, 30f), Random.Range(-30f, 30f)) * currentDirection;
        }

        // Apply changes to the texture
        generatedTexture.Apply();

        // Assign the generated texture to a material
        GetComponent<Renderer>().material.mainTexture = generatedTexture;
    }

    void DrawLineOnTexture(Vector2 startUV, Vector2 endUV, Color color)
    {
        int startX = Mathf.RoundToInt(startUV.x * textureWidth);
        int startY = Mathf.RoundToInt(startUV.y * textureHeight);
        int endX = Mathf.RoundToInt(endUV.x * textureWidth);
        int endY = Mathf.RoundToInt(endUV.y * textureHeight);

        // Draw line segment on the texture
        BresenhamLine(startX, startY, endX, endY, color);
    }

    void BresenhamLine(int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            // Set pixel color
            generatedTexture.SetPixel(x0, y0, color);

            if (x0 == x1 && y0 == y1)
                break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    Vector2 GetUVFromSpherePosition(Vector3 position)
    {
        float u = Mathf.Atan2(position.z, position.x) / (2 * Mathf.PI) + 0.5f;
        float v = Mathf.Acos(position.y) / Mathf.PI;

        return new Vector2(u, v);
    }


}
