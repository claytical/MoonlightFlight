using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{

    public Transform[] itemsToPingPongPosition;
    private Vector3[] originalPosition;
    private Vector3[] furthestPosition;
    public Vector2 distanceToTravel;
    public float travelSpeed;
    private int initialDirection = 1;
    public enum MovementType { Alternating, Forward, Reverse}
    public enum AlternatingTendency { Together, Apart }
    public MovementType movement;
    public AlternatingTendency tendency;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = new Vector3[itemsToPingPongPosition.Length];
        furthestPosition = new Vector3[itemsToPingPongPosition.Length];

        if (movement == MovementType.Reverse)
        {
            initialDirection = -1;
        }

        for (int i = 0; i < itemsToPingPongPosition.Length; i++)
        {

            furthestPosition[i] = itemsToPingPongPosition[i].position;
            furthestPosition[i].x += distanceToTravel.x * initialDirection;
            furthestPosition[i].y += distanceToTravel.y * initialDirection;
            originalPosition[i] = itemsToPingPongPosition[i].position;
            if(movement == MovementType.Alternating)
            {
                initialDirection *= -1;
            }
            if(movement == MovementType.Forward)
            {
                initialDirection = 1;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < itemsToPingPongPosition.Length; i++)
        {
            if(tendency == AlternatingTendency.Apart)
            {
                itemsToPingPongPosition[i].position = Vector3.Lerp(furthestPosition[i], originalPosition[i], Mathf.Sin(Time.frameCount * travelSpeed));

            }
            else
            {
                itemsToPingPongPosition[i].position = Vector3.Lerp(originalPosition[i], furthestPosition[i], Mathf.Sin(Time.frameCount * travelSpeed));

            }


        }

    }
}
