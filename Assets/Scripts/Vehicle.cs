using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Vehicle : MonoBehaviour
{
    public Color starColor;
    public ParticleSystem energyCollection;
    public GameObject explosion;
    public bool lootDropped = false;
    public float force;
    public float maxForce;
    public GameObject touchPoint;
    public VehicleType type;
    public float timeStuck;
    public bool lootAvailable = false;
    public int maxHP;
    public float glitchAmount = .1f;
    public int currentHP;
    public int lootDropFrequency;
    private int energyCollected;

    private List<GameObject> touchPoints;
    private bool canPassThroughObjects;
    private bool isDead;
    private bool deadShip;
    private bool hyperBrake = false;
    private float hyperBrakeTimer;
    public SetInfo set;
    private ProceduralLevel track;
    private float yOffset;
    public RectTransform offLimitTouchPoint;

    // Start is called before the first frame update
    void Start()
    {
        touchPoints = new List<GameObject>();
        GetComponent<SpriteRenderer>().color = starColor;
        energyCollected = 0;
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        timeStuck += Time.deltaTime;
    }


    public void SelfDestruct()
    {
        isDead = true;
    }

    
    public void SetTrack(ProceduralLevel t)
    {
        track = t;
    }
   
    public void LinkSet(SetInfo s)
    {
        set = s;
        set.SetVehicle(this);
    }

    public void ApplyHyperBreak()
    {
        hyperBrake = true;
        hyperBrakeTimer = Time.time + 1f;
    }

    public void Stasis(bool active)
    {
        if(active)
        {
            if(GetComponent<Rigidbody2D>())
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().isKinematic = true;
                TurnOffCollision();                
            }
        }
        else
        {
            if(GetComponent<Rigidbody2D>())
            {
                TurnOnCollision();
                GetComponent<Rigidbody2D>().isKinematic = false;

            }
        }
    }

    public void TurnOffCollision()
    {
        if (GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = false;

        }
        if (GetComponent<CircleCollider2D>())
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }

        if (GetComponent<PolygonCollider2D>())
        {
            GetComponent<PolygonCollider2D>().enabled = false;
        }

    }

    public void TurnOnCollision()
    {
        if (GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().enabled = true;

        }

        if (GetComponent<CircleCollider2D>())
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }

        if (GetComponent<PolygonCollider2D>())
        {
            GetComponent<PolygonCollider2D>().enabled = true;
        }

    }

    void FixedUpdate()
    {
        CheckControl();

        if (hyperBrake)
        {
            if(hyperBrakeTimer > Time.time)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().rotation = 0;
            }
            else
            {
                hyperBrake = false;
            }

        }
        else
        {
            if (force > maxForce)
            {
                force = maxForce;
            }

        }

        if (timeStuck > 1)
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

    void ResetGlitch()
    {
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift= 0f;
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = 0f;

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Collision!");
        if(coll.gameObject.GetComponent<Platform>())
        {
            Debug.Log("Hitting platform");
            if(coll.gameObject.GetComponent<Platform>().causesGlitch) {
                Debug.Log("Causes glitch");
                if(coll.gameObject.GetComponent<Hazard>())
                {
                    //hazard glitch
                    Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift = glitchAmount * 2f;
                    Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = glitchAmount;
                    Invoke("ResetGlitch", .1f);

                }
                else
                {
                    //standard bump glitch
                    Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift = glitchAmount;
                    Invoke("ResetGlitch", .05f);

                }

                //                Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = .5f;


            }

        }
        if(coll.gameObject.GetComponent<Hull>())
        {
            Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = glitchAmount * .5f;
            //                Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = .5f;

            Invoke("ResetGlitch", .05f);

        }
        /*
         * if(GetComponentInParent<Animator>())
                {

                    GetComponentInParent<Animator>().SetTrigger("glitch");
                }
        */
        if (coll.gameObject.GetComponent<SpawnsObjects>())
        {
            if (coll.gameObject.GetComponent<SpawnsObjects>().collisionCausesSpawn)
            {
                coll.gameObject.GetComponent<SpawnsObjects>().SpawnObject();

            }
        }

        if (coll.gameObject.tag == "Disappearing")
        {
            energyCollected++;
            if(energyCollected%lootDropFrequency == 0)
            {
                track.LootDrop();
            }

            if (coll.gameObject.GetComponent<Breakable>())
            {
                GetComponentInParent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<Breakable>().hit);
                energyCollection.Play();


                if (coll.gameObject.GetComponent<Breakable>().isDead())
                {
                    //EVENT #1 - BROKE OBJECT
                    GetComponentInParent<ParkingLot>().EnergyCollected();
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
                        GetComponentInParent<ParkingLot>().GiveFeedback("Armor Collected!");
                        int armor = PlayerPrefs.GetInt("armor", 0);
                        armor++;
                        PlayerPrefs.SetInt("armor", armor);

                        //                        ToggleShield();
                        break;
//                    case PowerUp.Reward.Boundary:
//                        GetComponentInParent<ParkingLot>().GiveFeedback("Perimeter Fortified!");
//                       GetComponentInParent<ParkingLot>().boundaries.AddBorders(2);

//                        break;
                    case PowerUp.Reward.Part:
                        GetComponentInParent<ParkingLot>().GiveFeedback("Ship Part Located!");
                        int parts = PlayerPrefs.GetInt("parts", 0);
                        parts++;
                        PlayerPrefs.SetInt("parts", parts);
                        break;

//                    case PowerUp.Reward.PassThrough:
//                        GetComponentInParent<ParkingLot>().GiveFeedback("Ship Phasing Enabled!");
//                        SetPassThrough(true);
//                        break;

                    case PowerUp.Reward.Consciousness:
                        GetComponentInParent<ParkingLot>().GiveFeedback("Consciousness Elevated!");
                        int consciousness = PlayerPrefs.GetInt("consciousness", 0);
                        consciousness++;
                        PlayerPrefs.SetInt("consciousness", consciousness);
                        break;
                    case PowerUp.Reward.Stop:
                        //                        ApplyHyperBreak();
                        int brake = PlayerPrefs.GetInt("brake", 0);
                        brake++;
                        PlayerPrefs.SetInt("brake", brake);
                        GetComponentInParent<ParkingLot>().GiveFeedback("Hyper Brake Collected!");
                        break;
                    case PowerUp.Reward.Nuke:
                        int nukes = PlayerPrefs.GetInt("nukes", 0);
                        nukes++;
                        GetComponentInParent<ParkingLot>().GiveFeedback("Nuke Collected!");
                        break;
                    case PowerUp.Reward.Warp:
                        //activate warp menu
                        track.warpMenu.SetActive(true);
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        break;
                }
                Destroy(coll.gameObject);
//                SetEnergyLevel(0);
//hasPowerup = false;
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

            if(coll.gameObject.GetComponent<Hull>().gauge.TakeDamage())
            {
                isDead = true;
            }
            }

        if (coll.gameObject.tag == "Avoid")
        {

            if (coll.gameObject.GetComponent<Remix>().level.lot.HP.TakeDamage())
            {
                isDead = true;
            }


            //TODO: CHECK FOR SHIELD
        }

        gameObject.GetComponentInParent<AudioSource>().Play();
    }
    /*
    public bool HasFullEnergy() {
        if(currentEnergyLevel >= maxEnergy)
        {
            return true;
        }
        return false;
    }
    */
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
            Vector2 direction = (Vector2)touchPosition - (Vector2)transform.position;
            direction.Normalize();
            
            if (Input.mousePosition.x > offLimitTouchPoint.offsetMin.x && Input.mousePosition.y < offLimitTouchPoint.offsetMax.y)
            {
                //no touch zone
                Debug.Log("Touch Not Allowed");
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
            }

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
