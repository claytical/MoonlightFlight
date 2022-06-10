using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standard : MonoBehaviour
{
    public Sprite icon;
    public void Finished()
    {
        //CALLED IN INITIAL ANIMATION
        if(GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = true;

        }

        if (GetComponent<PolygonCollider2D>())
        {
            GetComponent<PolygonCollider2D>().enabled = true;

        }


        if (GetComponent<CircleCollider2D>())
        {
            GetComponent<CircleCollider2D>().enabled = true;

        }

    }
}
