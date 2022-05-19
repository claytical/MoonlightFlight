using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool finished = false;
    private Vector3 originalScale;
    public bool canBePushed = false;
    public bool canTurnHazardous = false;
    public bool canSpawnNewObjects;
    public GameObject[] objectsToSpawn;
    public Transform[] placesToSpawn;
    public RigidbodyConstraints2D constraints;
    public float gravity;

    private bool isMovingFromPush = false;
    private Vector3 originalPosition;
    private Vector3 pushToPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        GetComponent<BoxCollider2D>().enabled = false;   
        if(canBePushed)
        {
            originalPosition = transform.position;
        }
        SetConstraints();
    }


    private void switchTag()
    {
        gameObject.tag = "Avoid";

    }
    public void TurnHazardous()
    {
        Invoke("switchTag", 1);
        //TODO: Change to custom sprite
        GetComponent<Remix>().hazard.color = GetComponent<Remix>().level.hazardColor;
        GetComponent<Remix>().primary.enabled = false;
        GetComponent<Remix>().hazard.transform.localScale = Vector3.one;
        GetComponent<Animator>().enabled = false;
        Debug.Log("turned hazardous");
    }

    // Update is called once per frame

    public void SpawnObject()
    {
        Vector3 newPosition = transform.position;
        newPosition.y += -1;
        Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Length)],newPosition, Quaternion.identity, transform.parent);
    }

    void Update()
    {
        if(isMovingFromPush)
        {
        }
    }

    public void SetConstraints()
    {
        GetComponent<Rigidbody2D>().constraints = constraints;
        GetComponent<Rigidbody2D>().gravityScale = gravity;
    }

    public void Finished()
    {
        //CALLED IN INITIAL ANIMATION
        finished = true;
        transform.localScale = originalScale;
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
