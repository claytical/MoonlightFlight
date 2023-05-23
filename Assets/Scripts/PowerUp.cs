using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowColorLerp : MonoBehaviour
{
    public Color[] rainbowColors;
    public float duration = 1f;
    public Renderer rend;

    private float lerpTime;
    private int colorIndex;

    public void Lerp()
    {
        // increment lerp time
        lerpTime += Time.deltaTime;

        // check if we need to switch to the next color
        if (lerpTime >= duration)
        {
            lerpTime = 0f;
            colorIndex = (colorIndex + 1) % rainbowColors.Length;
        }

        // calculate lerp amount
        float lerpAmount = lerpTime / duration;

        // lerp between the current color and the next color
        Color currentColor = rainbowColors[colorIndex];
        Color nextColor = rainbowColors[(colorIndex + 1) % rainbowColors.Length];
        Color lerpedColor = Color.Lerp(currentColor, nextColor, lerpAmount);
        lerpedColor.a = 1f;
        Debug.Log("COLOR: " + lerpedColor);

        // apply the lerped color to the renderer
        rend.material.color = lerpedColor;
    }
}

public class PowerUp : MonoBehaviour
{
    [System.Serializable]
    public enum Reward
    {
        Shield,
        Part,
        Stop,
        Nuke,
        Warp
    };

    public Reward reward;
    public SpriteRenderer icon;
    public SpriteRenderer border;
    public int timesAround = 2;
    private int timesAroundCounter = 0;
    private bool spinning = false;
    private float[] spinTime;
    private int spindex;
    private Sprite[] possibleItems;
    public CircleCollider2D powerUpCollider;
    private Color itemBorder;
    private Sprite item;
    private Color originalItemColor;
    private RainbowColorLerp rainbowColorLerp;
    private void Start()
    {
        Debug.Log("ICON IS " + icon.name);
        item = icon.sprite;
        originalItemColor = icon.color;
        itemBorder = border.color;
    }

     public void Spin(Sprite[] availableItems, float displayTime, bool easing = false)
    {
        powerUpCollider.enabled = false;
        rainbowColorLerp = new RainbowColorLerp();
        rainbowColorLerp.rainbowColors = new Color[] {
            Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta
        };
        rainbowColorLerp.duration = .1f;
        rainbowColorLerp.rend = border;
        spinTime = new float[availableItems.Length];
        possibleItems = new Sprite[availableItems.Length];
        for (int i = 0; i < availableItems.Length; i++)
        {
            if(easing)
            {
                spinTime[i] = Time.time + (displayTime * (i*i));
            }
            else
            {

            }
            spinTime[i] = Time.time + (displayTime * i);
        }
        possibleItems = availableItems;
        spinning = true;
        spindex = 0;
    }

    void Update()
    {
        if(spinning)
        {
            //            float h, s, v;
            //            Color.RGBToHSV(border.color, out h, out s, out v);

            // Use HSV values to increase H in HSVToRGB. It looks like putting a value greater than 1 will round % 1 it
            //            border.material.color = Color.HSVToRGB(h + Time.deltaTime * .25f, s, v);
            rainbowColorLerp.Lerp();
            if (spinTime[spindex] <= Time.time)
            {
                icon.sprite = possibleItems[spindex];
                spindex++;
                if (spindex >= possibleItems.Length)
                {
                    timesAroundCounter++;
                    if(timesAroundCounter > timesAround)
                    {
                        powerUpCollider.enabled = true;
                        spinning = false;
                        icon.sprite = item;
                        border.enabled = false;
                        icon.color = originalItemColor;

                    }
                    else
                    {
                        Spin(possibleItems, .15f, true);
                        timesAroundCounter++;
                    }
                }

            }
        }
    }

}
