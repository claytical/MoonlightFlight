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
    private int galaxyState = 0;
    // Start is called before the first frame update
    void Start()
    {
        galaxy.SetRelease(this);   
    }

    // Update is called once per frame
    void Update()
    {
        if(galaxyState < 2)
        {
            CheckControl();
        }
    }

    public void StopCheckingControls()
    {
        galaxyState = 2;
    }
    void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0) && !galaxy.letsMakePlanets)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = gameObject.transform.position.z;

            // place object where the player touched

            if (galaxyState == 0)
            {
                CreateEnergyPointAndMakePlanets(Input.mousePosition);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            galaxyState = 1;
            HideText();
            Destroy(currentTouchObject,2f);
        }

    }

    void HideText()
    {
        galaxy.ui.enabled = false;
        instructions.enabled = false;

    }

    void CreateEnergyPointAndMakePlanets(Vector3 position)
    {
        currentTouchObject = (GameObject)Instantiate(touchObject, position, transform.rotation);
        currentTouchObject.transform.parent = transform.parent;
        galaxy.letsMakePlanets = true;
        Camera.main.gameObject.GetComponent<AudioSource>().Stop();

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
                touchPosition.z = gameObject.transform.position.z;

                if (galaxyState == 0)
                {
                    CreateEnergyPointAndMakePlanets(touch.position);
                    HideText();
                }


            }

            if (touch.phase == TouchPhase.Ended)
            {
                galaxyState = 1;
                Destroy(currentTouchObject, 2f);
            }

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
