using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTransfer : MonoBehaviour
{
    public Transform startingPoint;
    public Transform endingPoint;
    public float speed = .1f;
    private float progress = 0;
    private Vector3 currentPosition;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (startingPoint && endingPoint)
        {
            
            currentPosition = Vector3.Lerp(startingPoint.position, endingPoint.position, progress);
            transform.position = currentPosition;
            Debug.Log("CURRENT POSITION:" +currentPosition);
            progress += speed;
        }

        if (progress >= 1)
        {
            Destroy(this.gameObject);
        }
    }
}
