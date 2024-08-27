using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject particles;
    bool ascending = false;
    // Start is called before the first frame update
    void Start()
    {
        GamepadManager.Instance.ResetPortal();
    }

    // Update is called once per frame
    void Update()
    {
        if(ascending)
        {
/*
            if(!particles.GetComponent<ParticleSystem>().isEmitting)
            {
                Debug.Log("HIDE PORTAL");
                GamepadManager.Instance.HidePortal();
           //     Destroy(transform.parent.gameObject);
            }
            else
            {
                Debug.Log("PORTAL ACTIVE");

            }
*/




        }

    }

    public void Complete()
    {
        GamepadManager.Instance.HidePortal();
    }

    public bool isEmitting()
    {
        return particles.GetComponent<ParticleSystem>().isEmitting;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.GetComponent<Vehicle>())
        {
            Debug.Log("Vehicle Passed Through");
            if(coll.gameObject.GetComponentInParent<LocalPlayer>())
            {
                //LOCAL PLAYER PASSED THROUGUH
                coll.gameObject.GetComponentInParent<LocalPlayer>().NextPlane();
                if(GamepadManager.Instance.CheckAstralPlaneAlignment())
                {
                    GetComponentInParent<RotateConstant>().accelerate = new Vector3(0, 0, 5);
                    particles.SetActive(true);
                    GetComponent<SpriteRenderer>().enabled = false;
                    ascending = true;
                }
                
            }
        }
    }

}