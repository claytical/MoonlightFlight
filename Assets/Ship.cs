using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private int currentEnergyLevel;
    public Energy[] meter;
    public GameObject energy;
    public bool poweredUp = false;
    private List<GameObject> touchPoints;
    private bool canPassThroughObjects;
    private bool isDead;
    private bool deadShip;
    private bool hasPowerup = false;
    private LevelGrid grid;
    private EndlessLevel level;
    private float yOffset;

    public float force;
    public float maxForce;
    public GameObject touchPoint;

    // Start is called before the first frame update
    void Start()
    {
        yOffset = energy.transform.position.y;
        //        meter = new List<Energy>();
        touchPoints = new List<GameObject>();
        
        for(int i = 0; i < meter.Length; i++) {
            meter[i].Deactivate();
        }
        currentEnergyLevel = 0;

    }

    public void powerDown()
    {
        Debug.Log("POWERING DOWN!");
        for (int i = meter.Length - 1; i > 0; i--)

        {
            //TODO: set transparency instead
            meter[i].Deactivate();
            
        }
        SetEnergyLevel(0);
        grid.LowEnergy();
        SetPassThrough(false);
        grid.PlatformTransparency(false);
        hasPowerup = false;
    }

    public void addEnergy(int amount)
    {
        if (currentEnergyLevel + amount <= meter.Length)
        {
            //has less than the amount of energy to go full
            for (int i = currentEnergyLevel; i < currentEnergyLevel + amount; i++)
            {
                meter[i].Activate();
            }
        }
        else
        {
            //has all the energy to go full
            for (int i = currentEnergyLevel; i < meter.Length; i++)
            {
                meter[i].Activate();
            }
        }

        currentEnergyLevel += amount;

        if (HasFullEnergy() && !hasPowerup && !grid.fullEnergy.isPlaying) {
            //EVENT #3
            level.MaxEnergyReached();
            hasPowerup = true;

        }
    }

    public void SetLevel(EndlessLevel l)
    {
        level = l;
    }

    public void SetEnergyLevel(int amount)
    {
        currentEnergyLevel = amount;

    }

    /*
    void CheckEnergy()
    {
        //TODO: 
        int energySpaces = meter.Count;



    }
    */
    public void LinkGrid(LevelGrid g)
    {
        grid = g;
        grid.SetShip(this);
    }

    // Update is called once per frame
    void Update()
    {
        CheckControl();
        if (force > maxForce)
        {
            force = maxForce;
        }

        if (isDead && !deadShip)
        {
            deadShip = true;
            Destroy(gameObject, 4);
            level.GameOver();
        }
        Vector3 energyPos = transform.position;
        energyPos.y = energyPos.y + yOffset;
        energy.transform.position = energyPos;
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Disappearing")
        {
            if (!HasFullEnergy())
            {
                addEnergy(1);
            }

            if (coll.gameObject.GetComponent<Breakable>())
            {
                GetComponentInParent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<Breakable>().hit);
                if (!coll.gameObject.GetComponent<Breakable>().LightUp(this.gameObject))
                {
                    //EVENT #1 - BROKE OBJECT
                    grid.currentSet.BroadcastMessage("BrokeObject", SendMessageOptions.DontRequireReceiver);

                }
            }

            if (coll.gameObject.GetComponent<BreakablePlatform>())
            {
                coll.gameObject.GetComponent<BreakablePlatform>().Bumped(this.gameObject);

            }
        }

        if (coll.gameObject.tag == ("Power Up"))
        {
        

            if (coll.gameObject.GetComponent<PowerUp>())
            {

                switch (coll.gameObject.GetComponent<PowerUp>().reward)
                {
                    case PowerUp.Reward.Shield:
//                        ToggleShield();
                        break;
                    case PowerUp.Reward.Boundary:
                        GetComponentInParent<Dock>().boundaries.AddBorders(3);

                        break;
                    case PowerUp.Reward.SpeedUp:
                        force *= 1.5f;
                        break;

                    case PowerUp.Reward.SlowDown:
                        force *= .5f;
                        break;

                    case PowerUp.Reward.PassThrough:
                        SetPassThrough(true);
                        break;

                }
                Destroy(coll.gameObject);

            }

        }

        if (coll.gameObject.tag == "Bumpable" && canPassThroughObjects)
        {
            Debug.Log("I'm going through a bumpable object");
        }

        if (coll.gameObject.tag == "Bumpable" && !canPassThroughObjects)
        {
            //ADD SOUND
            GetComponentInParent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<CollisionSound>().soundFx[0], .5f);
            Debug.Log("Bumped Object");
            //EVENT #2 - BUMPED PLATFORM
            grid.currentSet.BroadcastMessage("BumpedPlatform", SendMessageOptions.DontRequireReceiver);
            //Check for polygon shenanigans
            //frameCountAtBump = Time.frameCount + framesUntilTilt;
            if (coll.gameObject.GetComponent<Animator>())
            {
                coll.gameObject.GetComponent<Animator>().SetTrigger("hit");
            }
        }


        if (coll.gameObject.tag == "Boundary")
        {
            Debug.Log("Hit Boundary");
            isDead = coll.gameObject.GetComponentInParent<BoundaryPowerUp>().Hit();
        }

        if (coll.gameObject.tag == "Avoid")
        {
            //TODO: CHECK FOR SHIELD
            if (false)
            {
                isDead = GetComponent<Shield>().Hit(1);
                Debug.Log("Your shield is now  " + isDead);

            }
        }

        gameObject.GetComponentInParent<AudioSource>().Play();
    }

    public bool HasFullEnergy() {
        for (int i = 0; i < meter.Length; i++)
        {
            if (!meter[i].isActive())
            {
                return false;
            }
        }
        
        return true;
    }

    public void SetPassThrough(bool active)
    {
        Debug.Log("Settting Transparency to " + active);
        canPassThroughObjects = active;
        grid.PlatformTransparency(active);
    }



    void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;
            GameObject go = (GameObject)Instantiate(touchPoint, touchPosition, transform.rotation);
            go.transform.parent = transform.parent;

            touchPoints.Add(go);
        }
        if (Input.GetMouseButtonUp(0))
        {
            GameObject tP = touchPoints[0];
            touchPoints.RemoveAt(0);
            Destroy(tP, 5);
        }

        if (Input.GetMouseButton(0))
        {

            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)touchPosition - (Vector2) transform.position;
            direction.Normalize();

            GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
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
                Debug.Log("Touch Began! " + touch.fingerId);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 10;
                GameObject go = (GameObject)Instantiate(touchPoint, touchPosition, transform.rotation);
                go.transform.parent = transform.parent;
                touchPoints.Add(go);
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
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            Vector2 direction = (Vector2)touchPosition - (Vector2) transform.position;
            direction.Normalize();
            GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
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
