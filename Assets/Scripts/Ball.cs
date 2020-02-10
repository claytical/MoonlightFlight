using UnityEngine;
using System.Collections;
//using UnityEngine.GameTune;

public class Ball : MonoBehaviour {

    public bool inPlay = false;
    public int framesUntilTilt;
    public float force;
    public float maxForce;



    /*
     * GAMETUNE POTENTIAL: 
     * BALL FORCE: 2, 5, 10, 15
     * GRID SELECTION (ARRAY, RANDOMIZE)
     * SHIELD STRENGTH
     * BORDER STRENGTH
     * NUMBER OF CLEARED GRIDS REQUIRED FOR ADVANCEMENT
     */
     
    public GameObject fly;
    public GameObject shield;
    public Light light;

    private int frameCountAtBump;
    private int timeSinceLastBump = 0;

    private Vector3 warpPosition;
	private Vector3 originalPosition;
    private Vector3 calibratedTilt;
    private Vector3 tilt;

    private bool canTilt = true;
    private bool usingTiltControls = false;
    private bool hasShield;
    private bool isDead = false;
    private bool isWarping = false;
    private bool canPassThroughObjects = false;
    private bool deadBall = false;

    //    public ProceduralSet currentSet;
    public Grid grid;
    
    public void ToggleShield()
    {
        shield.SetActive(!shield.activeSelf);

    }

    public void SetPassThrough(bool active)
    {
        Debug.Log("Settting Transparency to " + active);
        canPassThroughObjects = active;
        GetComponentInParent<BallHolder>().grid.PlatformTransparency(active);
    }

    // Use this for initialization

    
    void Start () {
//        Question speed = GameTune.CreateQuestion("BallSpeed", new string[] { "medium", "fast", "slow" }, SpeedAnswerHandler);

        originalPosition = transform.position;
        if (PlayerPrefs.GetInt("tilt") == 1)
        {
            usingTiltControls = true;
        }
        else
        {
            usingTiltControls = false;
        }
	}
    /*
    private void SpeedAnswerHandler(Answer answer)
    {
        Debug.Log("Using " + answer.Value);
        switch (answer.Value) {
            case "medium":
                force = 5f;
                break;
            case "fast":
                force = 10f;
                break;
            case "slow":
                force = 2f;
                break;
        }

        answer.Use();
    }
    */
public void Calibrate()
    {
        //Gets devices physical rotation in 3D space

        calibratedTilt.x = Input.acceleration.x;
        calibratedTilt.y = Input.acceleration.y;
        calibratedTilt.z = Input.acceleration.z;
    }
    // Update is called once per frame

    public void SetTiltControls(bool status)
    {
        usingTiltControls = status;
    }
    public bool UsingTiltControls()
    {
        return usingTiltControls;
    }

    void FixedUpdate()
    {
        if (inPlay && canTilt && usingTiltControls)
        {


            //Accelerometer
            //get input from accelerometer

            tilt.x = Input.acceleration.x - calibratedTilt.x;
            tilt.y = Input.acceleration.y - calibratedTilt.y;
            tilt.z = -Input.acceleration.z - calibratedTilt.z;
            tilt = Quaternion.Euler(90, 0, 0) * tilt;
            GetComponent<Rigidbody2D>().velocity = new Vector2(tilt.x, tilt.y) * (force * .5f);

        


        }
        else if (inPlay && !canTilt && usingTiltControls)
        {
            if (frameCountAtBump <= Time.frameCount)
            {
                canTilt = true;
                GetComponent<Animator>().SetBool("tiltLocked", false);

            }
        }
        
    }
    
    void Update () {
        CheckSpeedLimit();
        timeSinceLastBump++;
        if (isDead && !deadBall) {
            deadBall = true;
			GetComponentInParent<BallHolder> ().DeadBall ();
			Destroy(gameObject, 4);

		}

		if (isWarping) {
			transform.position = Vector3.Lerp (transform.position, warpPosition, Time.deltaTime * 3f);
			if (Vector3.Distance (transform.position, warpPosition) <= .01f) {
				isWarping = false;
				GetComponent<Animator> ().SetTrigger ("warp");
			}
		}
	}
	void warped() {
		transform.position = originalPosition;
		gameObject.GetComponent<Rigidbody2D> ().simulated = true;
	}
	void warp(GameObject portal) {
		gameObject.GetComponent<Rigidbody2D> ().simulated = false;
		portal.GetComponent<Animator> ().SetTrigger ("warp");
		isWarping = true;
		warpPosition = portal.transform.position;
	}

	void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Warp")
        {
            Debug.Log("Warping!");
            warp(other.gameObject);
        }
        else
        {
            isDead = true;
            //GetComponentInParent<LevelSound>().NormalMode();
//            GetComponentInParent<BallHolder>().player.GameOver("Didn't warp!", seedsCollected);

        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Changing Score");
        if (coll.gameObject.tag == "Disappearing")
        {
            GetComponentInParent<BallHolder>().energy++;
            if (coll.gameObject.GetComponent<Breakable>())
            {

                GetComponent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<Breakable>().hit);
                if(coll.gameObject.GetComponent<Breakable>().LightUp(this.gameObject))
                {
                    //not dead
                    CheckEnergy(1);
                }
                else
                {
                    //dead
                    //EVENT #1 - BROKE OBJECT
                    Debug.Log("Hit something" + coll.gameObject);
                    grid.currentSet.BroadcastMessage("BrokeObject", SendMessageOptions.DontRequireReceiver);
                    CheckEnergy(2);

                }
            }

            if (coll.gameObject.GetComponent<BreakablePlatform>())
            {
                coll.gameObject.GetComponent<BreakablePlatform>().Bumped(this.gameObject);

            }
        }

        if(coll.gameObject.tag == ("Power Up"))
        {
            
            if (coll.gameObject.GetComponent<PowerUp>()) {
//                GameObject obj = GetComponentInParent<BallHolder>().player.level.CreatePowerUp();

                switch (coll.gameObject.GetComponent<PowerUp>().reward)
                {
                    case PowerUp.Reward.Shield:
                        ToggleShield();
                        break;
                    case PowerUp.Reward.Boundary:
                        GetComponentInParent<BallHolder>().player.boundary.GetComponent<BoundaryPowerUp>().SetBorderStrength(3);

                        break;
                    case PowerUp.Reward.SpeedUp:
                        force *= 2;
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

        if(coll.gameObject.tag == "Bumpable" && canPassThroughObjects)
        {
            Debug.Log("I'm going through a bumpable object");
        }
        if (coll.gameObject.tag == "Bumpable" && !canPassThroughObjects)
        {
            //ADD SOUND
            GetComponent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<CollisionSound>().soundFx[0],.5f);
            Debug.Log("Bumped Object");
            GetComponentInParent<BallHolder>().energy++;
            //EVENT #2 - BUMPED PLATFORM
            grid.currentSet.BroadcastMessage("BumpedPlatform", SendMessageOptions.DontRequireReceiver);
            //Check for polygon shenanigans
            frameCountAtBump = Time.frameCount + framesUntilTilt;
            if (coll.gameObject.GetComponent<Animator>())
            {
                coll.gameObject.GetComponent<Animator>().SetTrigger("hit");
            }
            if (canTilt)
            {
                canTilt = false;
                if (coll.gameObject.GetComponent<Animator>())
                {
                    GetComponent<Animator>().SetBool("tiltLocked", true);
                }
            }
            //coll.gameObject.GetComponent<Immovable> ().LightUp ();
            CheckEnergy(1);
        }
        
        
        if (coll.gameObject.tag == "Boundary")
        {
            Debug.Log("Hit Boundary");
            isDead = coll.gameObject.GetComponentInParent<BoundaryPowerUp>().Hit();
            /*
            if (isDead)
            {
                GetComponentInParent<BallHolder>().player.GameOver("You flew out of bounds!", seedsCollected);

            }
            */
        }
   
        if (coll.gameObject.tag == "Avoid")
        {
            if (hasShield)
            {
                isDead = GetComponent<Shield>().Hit(1);
                Debug.Log("Your shield is now  " + isDead);

            }
        }
    
            timeSinceLastBump = 0;
            gameObject.GetComponent<AudioSource>().Play();
    }

    void CheckSpeedLimit()
    {
        if(force > maxForce)
        {
            force = maxForce;
        }
    }

    void CheckEnergy(int amount = 0)
    {
        int speedState = GetComponentInParent<BallHolder>().increaseEnergy(amount);
        light.intensity = Mathf.InverseLerp(0, 25, GetComponentInParent<BallHolder>().energy);
        if (speedState > 0)
        {
            force += 1;
            if (speedState == 2)
            {
                //EVENT #3 - MAX ENERGY REACHED

             //   GetComponentInParent<BallHolder>().MaxEnergy();
            }
        }

    }
    public void Shrink()
    {
        gameObject.transform.localScale = gameObject.transform.localScale * .5f;
    }

    public void Enlarge()
    {
        gameObject.transform.localScale = gameObject.transform.localScale * 2f;

    }
}
