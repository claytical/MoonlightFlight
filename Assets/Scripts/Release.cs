using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Release : MonoBehaviour
{

    public GameObject touchObject;
    private GameObject currentTouchObject;
//    public GenerateObjects lightGenerator;
    public Galaxy galaxy;
    public Text instructions;
    private bool startedCreation = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            CheckControl();
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0) && !galaxy.letsMakePlanets)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = gameObject.transform.position.z;

            // place object where the player touched

            if (!startedCreation)
            {
                currentTouchObject = (GameObject)Instantiate(touchObject, touchPosition, transform.rotation);
                currentTouchObject.transform.parent = transform.parent;
                //          lightGenerator.creatingObjects = true;
                galaxy.letsMakePlanets = true;
                galaxy.ui.enabled = false;
                instructions.enabled = false;
                Camera.main.gameObject.GetComponent<AudioSource>().Stop();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            startedCreation = true;

            Destroy(currentTouchObject,2f);
        }

    }

    void CheckTouches()
    {
        Touch touch = new Touch();

        for (int i = 0; i < Input.touchCount; i++)
        {
            touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                GetComponentInParent<AudioSource>().Play();
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 10;
                GameObject go = (GameObject)Instantiate(touchObject, touchPosition, transform.rotation);
                go.transform.parent = transform.parent;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                //                touchPoints[i].transform.position = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                //       GameObject tP = touchPoints[i];
                //       touchPoints.RemoveAt(i);
                //       Destroy(tP, 5);
            }

        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            //apply physics
/*
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            Vector2 direction = (Vector2)touchPosition - (Vector2)transform.position;
            direction.Normalize();
            GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
  */
            }

    }


    void CheckControl()
    {
        if (Input.mousePresent)
        {
            CheckMouse();
        }
        CheckTouches();

    }

}
