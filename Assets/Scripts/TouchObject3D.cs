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

public struct FocusPoint
{
    public Vector3 focusCoordinates;
    public Quaternion focusRotation;
    public MovingEntity[] movingEntities;
    public string conversation;

}

public class TouchObject3D : MonoBehaviour
{

    FocusPoint[] fPoints;
    public int focusPointIndex;
    public enum touchState
    {
        PARENT_SELECTED = 0,
        CHILD_SELECTED = 1
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Focus(int index)
    {
        
        Camera.main.transform.position = fPoints[index].focusCoordinates;
        Camera.main.transform.rotation = fPoints[index].focusRotation;
        for(int i = 0; i < fPoints[index].movingEntities.Length; i++)
        {
            fPoints[index].movingEntities[i].gameObject.transform.position = fPoints[index].movingEntities[i].coordinates;
        }

        DialogueManager.StopConversation();
        DialogueManager.StartConversation(fPoints[index].conversation);
        if(DialogueManager.HasInstance)
        {
            DialogueManager.SetDialoguePanel(true);
        }

    }
}
