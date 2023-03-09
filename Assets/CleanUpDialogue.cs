using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpDialogue : MonoBehaviour
{

    public GameObject[] objectToMove;
    public Vector3[] coordinatesToMoveObjectTo;
    public GameObject[] objectToHide;
    public GameObject[] objectToShow;
    public bool cleanUpOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if(cleanUpOnStart)
        {
            Clean();
        }
    }

    public void Clean()
    {
        for (int i = 0; i < objectToMove.Length; i++)
        {
            if (coordinatesToMoveObjectTo.Length >= i)
            {

                objectToMove[i].transform.position = coordinatesToMoveObjectTo[i];
            }
        }
        for (int i = 0; i < objectToHide.Length; i++)
        {
            objectToHide[i].SetActive(false);
        }
        for (int i = 0; i < objectToShow.Length; i++)
        {
            objectToShow[i].SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
