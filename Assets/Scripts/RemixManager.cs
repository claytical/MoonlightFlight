using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemixManager : MonoBehaviour
{
    public Color platform;
    public Color hazard;
    public Color collectable;
    public Color breakable;

    private int currentThemeIndex;

    // Start is called before the first frame update
    void Start()
    {
        Remix[] remixed = Resources.FindObjectsOfTypeAll<Remix>();
        for(int i = 0; i < remixed.Length; i++)
        {
            remixed[i].SetRemixManager(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // This could be used to cycle through themes or apply changes in real-time if needed
    }

    public void ApplyTheme(int themeIndex)
    {
        // Set the current theme index
        currentThemeIndex = themeIndex;

        // Apply colors based on the theme index
        switch (themeIndex)
        {
            case 0:
                platform = Color.green;
                hazard = Color.red;
                collectable = Color.yellow;
                breakable = Color.blue;
                break;

            case 1:
                platform = Color.magenta;
                hazard = Color.black;
                collectable = Color.cyan;
                breakable = Color.gray;
                break;

            // Add more themes as needed
            default:
                Debug.LogWarning("Theme index out of range. Applying default colors.");
//                ApplyDefaultColors();
                break;
        }

        // Apply these colors to game elements
        ApplyColorsToGameElements();
    }

    private void ApplyDefaultColors()
    {
        platform = Color.white;
        hazard = Color.red;
        collectable = Color.yellow;
        breakable = Color.gray;
    }

    private void ApplyColorsToGameElements()
    {
        // Example: Apply the colors to game objects (this will vary depending on your setup)
        // Assuming you have a way to reference these objects, e.g., by tags or specific GameObject names

        // Example pseudocode:
        // GameObject.Find("Platform").GetComponent<Renderer>().material.color = platform;
        // GameObject.Find("Hazard").GetComponent<Renderer>().material.color = hazard;
        // GameObject.Find("Collectable").GetComponent<Renderer>().material.color = collectable;
        // GameObject.Find("Breakable").GetComponent<Renderer>().material.color = breakable;

        // Apply colors to various elements based on your design
    }

    public void AdjustColors(float adjustmentFactor, bool lighten)
    {
        platform = AdjustColor(platform, adjustmentFactor, lighten);
        hazard = AdjustColor(hazard, adjustmentFactor, lighten);
        collectable = AdjustColor(collectable, adjustmentFactor, lighten);
        breakable = AdjustColor(breakable, adjustmentFactor, lighten);

        ApplyColorsToGameElements(); // Re-apply the adjusted colors to game elements
    }

    private Color AdjustColor(Color color, float adjustmentFactor, bool lighten)
    {
        adjustmentFactor = Mathf.Clamp01(adjustmentFactor); // Ensure the adjustmentFactor is between 0 and 1

        if (lighten)
        {
            // Lighten the color by interpolating towards white
            return Color.Lerp(color, Color.white, adjustmentFactor);
        }
        else
        {
            // Darken the color by multiplying RGB values
            return new Color(color.r * adjustmentFactor, color.g * adjustmentFactor, color.b * adjustmentFactor, color.a);
        }
    }
}
