using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTouchObject : MonoBehaviour
{
    public TouchObject3D parentTouchObject;
    // Update is called once per frame

    void Start()
    {
        parentTouchObject = GetComponentInParents<TouchObject3D>(transform);    
    }
    void Update()
    {
        CheckMouse();
        CheckTouch();
    }

    public static T GetComponentInParents<T>(Transform startTransform) where T : Component
    {
        Transform currentTransform = startTransform;

        while (currentTransform != null)
        {
            T component = currentTransform.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            currentTransform = currentTransform.parent;
        }

        return null;
    }

    private void CheckTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // get the touch position from the screen touch
            Vector3 touchPos = Input.GetTouch(0).position;

            // create a ray from the touch position
            Ray ray = Camera.main.ScreenPointToRay(touchPos);

            // create a RaycastHit object to store information about the collision
            RaycastHit hit;

            // check if the ray hits a 3D object
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {

                    // do something with the object that was hit
                    Debug.Log("Touched object: " + hit.transform.name);
                    //INDEX: CHILD STATE
                    parentTouchObject.Focus(0);

                }

            }
        }

    }



    private void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // get the mouse position
            Vector3 mousePos = Input.mousePosition;

            // create a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            // create a RaycastHit object to store information about the collision
            RaycastHit hit;

            // check if the ray hits a 3D object
            if (Physics.Raycast(ray, out hit))
            {
                // do something with the object that was hit
                if (hit.transform == transform)
                {
                    Debug.Log("Clicked object: " + hit.transform.name);
                    parentTouchObject.Focus();
                }
            }
        }

    }

}
