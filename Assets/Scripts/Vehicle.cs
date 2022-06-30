using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Vehicle : MonoBehaviour
{
    public int maxEnergy;
    public ParticleSystem energyCollection;
    public SpriteRenderer energyOverlay;
    public float energyUnit;
    public GameObject explosion;
    public bool poweredUp = false;
    public float force;
    public float maxForce;
    public GameObject touchPoint;
    public VehicleType type;
    public Shield shield;
    public float timeStuck;
    public GameObject energyTransfer;
    public bool lootAvailable = false;


    private int currentEnergyLevel;
    private List<GameObject> touchPoints;
    private bool canPassThroughObjects;
    private bool isDead;
    private bool deadShip;
    private bool hasPowerup = false;
    public SetInfo set;
    private ProceduralLevel track;
    private float yOffset;


    // Start is called before the first frame update
    void Start()
    {
        touchPoints = new List<GameObject>();
        currentEnergyLevel = 0;
        energyUnit = (float) maxEnergy / 100;
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        timeStuck += Time.deltaTime;
    }


    public void powerDown()
    {
        currentEnergyLevel = 0;
        Debug.Log("POWERING DOWN!");
        SetPassThrough(false);
        set.PlatformTransparency(false);
        hasPowerup = false;
    }

    public void addEnergy(int amount)
    {
        currentEnergyLevel += amount;
        Color energyColor = energyOverlay.color;
        energyColor.a = (energyUnit * currentEnergyLevel);
        energyOverlay.color = energyColor;
        if (HasFullEnergy() && !hasPowerup) {
            //EVENT #3
            track.MaxEnergyReached();
            hasPowerup = true;
            currentEnergyLevel = 0;
            energyColor.a = 0;
            energyOverlay.color = energyColor;

        }
    }

    public void SelfDestruct()
    {
        isDead = true;
    }

    
    public void SetTrack(ProceduralLevel t)
    {
        track = t;
    }

    public void SetEnergyLevel(int amount)
    {
        currentEnergyLevel = amount;

    }
   
    public void LinkSet(SetInfo s)
    {
        set = s;
        set.SetVehicle(this);
    }


    private void ToggleShield()
    {
        shield.gameObject.SetActive(true);
        shield.Setup();
    }

    void FixedUpdate()
    {
        CheckControl();

        if (force > maxForce)
        {
            force = maxForce;
        }

        if(timeStuck > 1)
        {
            Debug.Log("SHIP STUCK: " + timeStuck);
        }

        if (isDead && !deadShip)
        {
            Debug.Log("DEAD SHIP. GAME OVER");
            Vector3 pos = transform.position;
            Instantiate(explosion,pos, Quaternion.identity);
            track.GameOver();
            this.gameObject.SetActive(false);
            deadShip = true;
        }


    }


    void OnCollisionEnter2D(Collision2D coll)
    {


        if (coll.gameObject.GetComponent<SpawnsObjects>())
        {
            if (coll.gameObject.GetComponent<SpawnsObjects>().collisionCausesSpawn)
            {
                coll.gameObject.GetComponent<SpawnsObjects>().SpawnObject();

            }
        }

        if (coll.gameObject.tag == "Disappearing")
        {
            if (!HasFullEnergy())
            {
                addEnergy(1);
            }

            if (coll.gameObject.GetComponent<Breakable>())
            {
                GetComponentInParent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<Breakable>().hit);
                energyCollection.Play();


                if (coll.gameObject.GetComponent<Breakable>().isDead())
                {
                    //EVENT #1 - BROKE OBJECT
                    GetComponentInParent<ParkingLot>().PlanetCollected();
                    Destroy(coll.gameObject);

                }
                else
                {
                    Debug.Log("Just hitting an object, not destroyed.");

                }
            }

            if (coll.gameObject.GetComponent<BreakablePlatform>())
            {
                coll.gameObject.GetComponent<BreakablePlatform>().Bumped(this.gameObject);

            }
        }

        if (coll.gameObject.tag == "Power Up")
        {
            

            if (coll.gameObject.GetComponent<PowerUp>())
            {
                lootAvailable = false;
                GetComponentInParent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<Breakable>().hit);
                switch (coll.gameObject.GetComponent<PowerUp>().reward)
                {
                    case PowerUp.Reward.Shield:
                        GetComponentInParent<ParkingLot>().GiveFeedback("Shields Up!");
                        ToggleShield();
                        break;
                    case PowerUp.Reward.Boundary:
                        GetComponentInParent<ParkingLot>().GiveFeedback("Perimeter Fortified!");
                        GetComponentInParent<ParkingLot>().boundaries.AddBorders(2);

                        break;
                    case PowerUp.Reward.Part:
                        GetComponentInParent<ParkingLot>().GiveFeedback("Ship Part Located!");
                        int parts = PlayerPrefs.GetInt("parts", 0);
                        parts++;
                        PlayerPrefs.SetInt("parts", parts);
                        break;

                    case PowerUp.Reward.PassThrough:
                        GetComponentInParent<ParkingLot>().GiveFeedback("Ship Phasing Enabled!");
                        SetPassThrough(true);
                        break;

                    case PowerUp.Reward.Consciousness:
                        GetComponentInParent<ParkingLot>().GiveFeedback("Consciousness Elevated");
                        int consciousness = PlayerPrefs.GetInt("consciousness", 0);
                        consciousness++;
                        PlayerPrefs.SetInt("consciousness", consciousness);
                        break;

                }
                Destroy(coll.gameObject);
                SetEnergyLevel(0);
                hasPowerup = false;
            }

        }

        if (coll.gameObject.tag == "Bumpable" && canPassThroughObjects)
        {
            Debug.Log("I'm going through a bumpable object");
        }

        if (coll.gameObject.tag == "Bumpable" && !canPassThroughObjects)
        {
            if(coll.gameObject.GetComponent<Platform>())
            {
                if(coll.gameObject.GetComponent<Platform>().canBePushed)
                {
                    coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
                }

            }
            //PLAY COLLISION SOUND EFFECT
            GetComponentInParent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<CollisionSound>().soundFx[0], .5f);

            //EVENT #2 - BUMPED PLATFORM
            //THIS DOESN'T GO ANYWHERE?
            set.currentSet.BroadcastMessage("BumpedPlatform", SendMessageOptions.DontRequireReceiver);

            //CHECK TO SEE IF INSIDE?

            //Check for polygon shenanigans
            //frameCountAtBump = Time.frameCount + framesUntilTilt;
            if (coll.gameObject.GetComponent<Animator>())
            {
                coll.gameObject.GetComponent<Animator>().SetTrigger("hit");
            }
        }

        if (coll.gameObject.tag == "Boundary")
        {
            if (shield.isActiveAndEnabled)
            {
                Debug.Log("Using Shield!");
                shield.Hit(1);
            }
            else
            {

                isDead = coll.gameObject.GetComponentInParent<BoundaryPowerUp>().Hit();
            }
        }

        if (coll.gameObject.tag == "Avoid")
        {
            //TODO: CHECK FOR SHIELD
            if (shield.isActiveAndEnabled)
            {
                Debug.Log("Using Shield!");
                shield.Hit(1);
            }
            else
            {
                isDead = true;
            }
        }

        gameObject.GetComponentInParent<AudioSource>().Play();
    }

    public bool HasFullEnergy() {
        if(currentEnergyLevel >= maxEnergy)
        {
            return true;
        }
        return false;
    }

    public void SetPassThrough(bool active)
    {
        Debug.Log("Settting Transparency to " + active);
        canPassThroughObjects = active;
        set.PlatformTransparency(active);
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
            if (touchPoints.Count > 0)
            {
                GameObject tP = touchPoints[0];
                touchPoints.RemoveAt(0);
                Destroy(tP, 1);
            }
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
