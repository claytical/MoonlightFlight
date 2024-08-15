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
        HP,
        Fuel,
        Part
    };

    //Additional Power Ups

    //Energy/Loot Frequency Modifier?
    //Part Pack

    public Reward reward;
    public int amount = 1;
    public int timesAround = 2;
    public float timeLeft = 5f;

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
        timeLeft = Time.time + timeLeft;
    }

     public void Spin(Sprite[] availableItems, float displayTime, bool easing = false)
    {
        powerUpCollider.enabled = false;
        rainbowColorLerp = gameObject.AddComponent<RainbowColorLerp>();
        rainbowColorLerp.rainbowColors = new Color[] {
            Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta
        };
        rainbowColorLerp.duration = .1f;
        rainbowColorLerp.rend = GetComponent<SpriteRenderer>();
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


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<Vehicle>())
        {
            switch (reward)
            {
                case PowerUp.Reward.HP:
                    if (collision.gameObject.GetComponentInParent<Player>())
                    {
                        Debug.Log("Hit HP");
                        collision.gameObject.GetComponentInParent<Player>().IncreaseHP();
                    }
                    break;

                case PowerUp.Reward.Part:
//                    collision.gameObject.CollectPart(1);
                    break;
            }

            if(GetComponent<Explode>())
            {
                GetComponent<Explode>().Permanent();
            }

        }

    }

    void Update()
    {
        if(Time.time >= timeLeft)
        {
            Destroy(this.gameObject);
        }
        if(spinning)
        {
            rainbowColorLerp.Lerp();
            if (spinTime[spindex] <= Time.time)
            {
//                icon.sprite = possibleItems[spindex];
                spindex++;
                if (spindex >= possibleItems.Length)
                {
                    timesAroundCounter++;
                    if(timesAroundCounter > timesAround)
                    {
                        powerUpCollider.enabled = true;
                        spinning = false;
  /*                      icon.sprite = item;
                        border.enabled = false;
                        icon.color = originalItemColor;
  */
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
