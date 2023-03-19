using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

[System.Serializable]

public struct MovingEntity
{
    public GameObject gameObject;
    public Vector3 coordinates;
}


public class TouchObject3D : MonoBehaviour
{
    public Vector3 focusCoordinates;
    public Quaternion focusRotation;
    public MovingEntity[] movingEntities;
    public string conversation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouse();
        CheckTouch();
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
                    Focus();
    
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
                    Focus();
                }
            }
        }


    }

    public void Focus()
    {
        Debug.Log("FOCUSING ON " + gameObject.name);
        
        Camera.main.transform.position = focusCoordinates;
        Camera.main.transform.rotation = focusRotation;
        for(int i = 0; i < movingEntities.Length; i++)
        {
            movingEntities[i].gameObject.transform.position = movingEntities[i].coordinates;
        }

        DialogueManager.StopConversation();
        DialogueManager.StartConversation(conversation);
        DialogueManager.SetDialoguePanel(true);
    }
}
