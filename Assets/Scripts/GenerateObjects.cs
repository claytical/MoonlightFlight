using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObjects : MonoBehaviour
{

    public GameObject objectToGenerate;
    public int maxNumberOfObjectsToGenerate;
    private int numberOfObjectsGenerated = 0;
    public bool creatingObjects = false;
    private float height;
    private float width;
    // Start is called before the first frame update
    void Start()
    {
        //        float height = cam.orthographicSize; //  spawn on camera view border
        height = Camera.main.orthographicSize + 5; // now they spawn just outside
        width = height * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if(creatingObjects)
        {
            if(Time.frameCount%60 == 0 && maxNumberOfObjectsToGenerate >= numberOfObjectsGenerated)
            {
                GameObject go = (GameObject) Instantiate(objectToGenerate, new Vector3(Camera.main.transform.position.x + Random.Range(-width, width), 3, Camera.main.transform.position.z + height + Random.Range(10, 30)), Quaternion.identity);
                go.GetComponent<Firework>().SetTouchPoint(this.gameObject);
                numberOfObjectsGenerated++;
            }

        }
    }
}
