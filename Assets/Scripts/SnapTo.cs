using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTo : MonoBehaviour {
    public bool left;
    public bool right;
    public bool top;
    public bool bottom;
    // Use this for initialization
    void Start()
    {
        //        float startX = Screen.width / 2;
        //        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(startX, 0, 0));
        //        Vector3 origin = Camera.main.ViewportToScreenPoint(new Vector3(Screen.width/2, 0, 0));
        if (left) {
            Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));
            transform.position = new Vector3(origin.x-2f, origin.y, 1.5f);
         }
        if (right)
        {
            Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));
            transform.position = new Vector3(origin.x+2f, origin.y, 1.5f);
        }
        if (top)
        {
            Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, 0, 0));
            transform.position = new Vector3(origin.x, origin.y-2f, 1.5f);
        }
        if (bottom)
        {
            Vector3 origin = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height, 0));
            transform.position = new Vector3(origin.x, origin.y+2f, 1.5f);
        }


    }

    // Update is called once per frame
    void Update () {
		
	}
}
