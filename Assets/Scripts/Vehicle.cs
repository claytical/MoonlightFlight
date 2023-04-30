using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;



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
    public int energyCollectedBeforeLootDrop;
    private int energyCollected;

    private List<GameObject> touchPoints;
    private bool canPassThroughObjects;
    private bool isDead;
    private bool deadShip;
    private bool hyperBrake = false;
    private float hyperBrakeTimer;
    public SetInfo set;
    private ProceduralLevel level;
    private Playground playground;
    private float yOffset;
    public RectTransform offLimitTouchPoint;
    private string shipName;


    // Start is called before the first frame update
    void Start()
    {
        touchPoints = new List<GameObject>();
        GetComponent<SpriteRenderer>().color = starColor;
        energyCollected = 0;
        int parts = PlayerPrefs.GetInt("parts", 0);
        GetComponentInParent<ParkingLot>().partsUI.text = parts.ToString("0");

        //TODO: get ship name to append to cargo holds, also need to specify selected ship in the hangar
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        timeStuck += Time.deltaTime;
    }


    public void SelfDestruct()
    {
        Debug.Log("Self Destruct Activated");
        isDead = true;
    }

    
    public void SetLevel(ProceduralLevel t)
    {
        level = t;
    }

    public void SetPlayground(Playground p)
    {
        playground = p;
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
            if(level)
            {
                level.GameOver();
            }
            this.gameObject.SetActive(false);
            deadShip = true;
        }



    }

    void ResetGlitch()
    {
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift= 0f;
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = 0f;
        //add randomness to loot drop
        energyCollectedBeforeLootDrop = Random.Range(energyCollectedBeforeLootDrop - 1, energyCollectedBeforeLootDrop + 1);
        if (energyCollectedBeforeLootDrop < 2)
        {
            energyCollectedBeforeLootDrop = 2;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Collision!");
        if(coll.gameObject.GetComponent<Platform>())
        {
            if(coll.gameObject.GetComponent<Platform>().causesGlitch) {
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
            }

        }
        if(coll.transform.parent.GetComponent<Hull>())
        {
            Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = glitchAmount * .5f;
            //                Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = .5f;

            Invoke("ResetGlitch", .05f);

        }

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
            GetComponentInParent<ParkingLot>().energyUI.text = energyCollected.ToString("0");
            if(GetComponentInParent<ParkingLot>())
            {
                GetComponentInParent<ParkingLot>().energyUI.GetComponentInParent<Animator>().SetTrigger("collect");
            }
            if (energyCollected%energyCollectedBeforeLootDrop == 0)
            {
                if(level) {
                    level.LootDrop(coll.gameObject.transform);
                }
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
                        if(GetComponentInParent<ParkingLot>())
                        {
                            GetComponentInParent<ParkingLot>().HP.IncreaseHP();

                        }

                        
                        GetComponentInParent<ParkingLot>().GiveFeedback("Reinforcing Shields!");
                        //TODO: HP for current ship will always go down to 0 for destruction, but is it possible at the outpost
                        //to remove HP from one ship and give it to another? or does it come from using parts?
                        int armor = PlayerPrefs.GetInt("armor", 0);
                        armor++;
                        PlayerPrefs.SetInt("armor", armor);
                        break;
                                           
                    case PowerUp.Reward.Part:

                        GetComponentInParent<ParkingLot>().GiveFeedback("Ship Part Located!");
                        DialogueLua.SetVariable("Parts", DialogueLua.GetVariable("Parts").asInt + 1);
                        int parts = PlayerPrefs.GetInt("parts", 0);
                        parts++;
                        PlayerPrefs.SetInt("parts", parts);
                        GetComponentInParent<ParkingLot>().partsUI.text = parts.ToString("0");
                        if (GetComponentInParent<ParkingLot>())
                        {
                            GetComponentInParent<ParkingLot>().partsUI.GetComponentInParent<Animator>().SetTrigger("collect");
                        }

                        break;

                    case PowerUp.Reward.Stop:
                        DialogueLua.SetVariable("Brakes", DialogueLua.GetVariable("Brakes").asInt + 1);
                        GetComponentInParent<ParkingLot>().PowerUps.AddPowerUp(PowerUp.Reward.Stop, 1);
                        GetComponentInParent<ParkingLot>().GiveFeedback("Hyper Brake Collected!");
                        break;
                    case PowerUp.Reward.Nuke:
                        DialogueLua.SetVariable("Nukes", DialogueLua.GetVariable("Nukes").asInt + 1);
                        GetComponentInParent<ParkingLot>().PowerUps.AddPowerUp(PowerUp.Reward.Nuke, 1);
                        GetComponentInParent<ParkingLot>().GiveFeedback("Nuke Collected!");
                        break;
                    case PowerUp.Reward.Warp:
                        //activate warp menu
                        if(level)
                        {
                            level.warpMenu.SetActive(true);
                        }
                        if(playground)
                        {
                            playground.warpMenu.SetActive(true);
                        }
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        break;
                }
                Destroy(coll.gameObject);
            }

        }

        if (coll.gameObject.tag == "Bumpable" && canPassThroughObjects)
        {
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

            if (coll.gameObject.GetComponent<Animator>())
            {
                coll.gameObject.GetComponent<Animator>().SetTrigger("hit");
            }
        }

        if (coll.gameObject.tag == "Boundary")
        {
            if(coll.transform.parent.GetComponent<Hull>())
            {
                if (coll.transform.parent.GetComponent<Hull>().gauge.TakeDamage())
                {
                    isDead = true;
                }

            }
            else
            {
                Debug.Log("Hit non destructive boundary...");
            }
        }

        if (coll.gameObject.tag == "Avoid")
        {
            if (coll.gameObject.GetComponent<Explode>())
            {
                coll.gameObject.GetComponent<Explode>().Go();
            }
           if (coll.gameObject.GetComponent<Remix>().level.lot.HP.TakeDamage())  {
                coll.gameObject.GetComponent<Explode>().Go();
                isDead = true;
           }
        }

        gameObject.GetComponentInParent<AudioSource>().Play();
    }


    void CheckMouse()
    {

        if (Input.GetMouseButton(0))
        {


            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)touchPosition - (Vector2)transform.position;
            
            direction.Normalize();
            
            if ((RectTransformUtility.RectangleContainsScreenPoint(offLimitTouchPoint, Input.mousePosition))) { 

                //UI Area

                }
            
            else
                {
                GameObject go = (GameObject)Instantiate(touchPoint, touchPosition, transform.rotation);
                    go.transform.parent = transform.parent;
                    touchPoints.Add(go);
                    GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
                }

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

    }





    void CheckTouches()
    {
        Touch touch = new Touch();

        for (int i = 0; i < Input.touchCount; i++)
        {
            touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Stationary)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 10;
                if ((RectTransformUtility.RectangleContainsScreenPoint(offLimitTouchPoint, touch.position)))
                {


                }
                else
                {
                    GetComponentInParent<AudioSource>().Play();
                    GameObject go = (GameObject)Instantiate(touchPoint, touchPosition, transform.rotation);
                    go.transform.parent = transform.parent;
                    touchPoints.Add(go);
                    Vector2 direction = (Vector2)touchPosition - (Vector2)transform.position;
                    direction.Normalize();
                    GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);

                }
            }
        }
//TODO? Should player be able to drag finger?
/*
        for (int i = 0; i < Input.touchCount; i++)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            Vector2 direction = (Vector2)touchPosition - (Vector2) transform.position;
            direction.Normalize();
            GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        }
*/
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
