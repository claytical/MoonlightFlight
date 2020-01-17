using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	private int timeSinceLastBump = 0;
	private bool isDead = false;
    private bool isWarping = false;
    public bool inPlay = false;
    public int framesUntilTilt;
    private bool canTilt = true;
    private int frameCountAtBump;
	private Vector3 warpPosition;
	private Vector3 originalPosition;
    private Vector3 calibratedTilt;
    private Vector3 tilt;
    public float force;
    public GameObject fly;
//    public ProceduralSet currentSet;
    public Grid grid;
	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
	}


    public void Calibrate()
    {
        //Gets devices physical rotation in 3D space

        calibratedTilt.x = Input.acceleration.x;
        calibratedTilt.y = Input.acceleration.y;
        calibratedTilt.z = Input.acceleration.z;
    }
    // Update is called once per frame
    /*
    void FixedUpdate()
    {
        if (inPlay && canTilt)
        {


            //Accelerometer
            //get input from accelerometer

            tilt.x = Input.acceleration.x - calibratedTilt.x;
            tilt.y = Input.acceleration.y - calibratedTilt.y;
            tilt.z = -Input.acceleration.z - calibratedTilt.z;
            tilt = Quaternion.Euler(90, 0, 0) * tilt;
            GetComponent<Rigidbody2D>().velocity = new Vector2(tilt.x, tilt.y) * force;

        


        }
        else if (inPlay && !canTilt)
        {
            if (frameCountAtBump <= Time.frameCount)
            {
                canTilt = true;
                Debug.Log("Tilt Back");
                GetComponent<Animator>().SetBool("tiltLocked", false);

            }
        }
        
    }
    */
    void Update () {
		timeSinceLastBump++;

        if (isDead) {
			GetComponentInParent<BallHolder> ().DeadBall ();
			Destroy(gameObject);

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
            GetComponentInParent<BallHolder>().player.GameOver("Didn't warp!");

        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Disappearing")
        {
            if (coll.gameObject.GetComponent<Breakable>())
            {
                if(coll.gameObject.GetComponent<Breakable>().LightUp(this.gameObject))
                {
                    //not dead
                    CheckFever(1);
                }
                else
                {
                    //dead
                    //EVENT #1 - BROKE OBJECT
                    Debug.Log("Hit something" + coll.gameObject);
                    grid.currentSet.BroadcastMessage("BrokeObject", SendMessageOptions.DontRequireReceiver);
                    CheckFever(2);

                }
            }

            if (coll.gameObject.GetComponent<BreakablePlatform>())
            {
                coll.gameObject.GetComponent<BreakablePlatform>().Bumped(this.gameObject);

            }
        }

        if (coll.gameObject.tag == "Bumpable")
        {
            //EVENT #2 - BUMPED PLATFORM
            Debug.Log("Hit Platform");
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
            CheckFever(1);
        }
        
        if (coll.gameObject.tag == "Boundary")
        {
            isDead = true;
        //    GetComponentInParent<LevelSound>().NormalMode();
            GetComponentInParent<BallHolder>().player.GameOver("You flew out of bounds!");
        }
   
        if (coll.gameObject.tag == "Avoid")
        {
            //			gameObject.GetComponentInParent<BallHolder> ().removePoints ();
            //			coll.gameObject.GetComponent<AudioSource>().Play();
            //Trigger ball animation

            if (GetComponentInParent<BallHolder>().HasFever())
            {
                //EVENT #4 - Fever Broke
                GetComponentInParent<BallHolder>().BreakFever();
                GetComponent<Animator>().SetTrigger("fever");
                Debug.Log("Fever Broke by Losing Invincibility");
                grid.currentSet.BroadcastMessage("FeverBroke", SendMessageOptions.DontRequireReceiver);
            }

            else
            {
                isDead = true;
                GetComponentInParent<BallHolder>().player.GameOver("You hit a mine!");

            }
            //
            /*
            GetComponent<Animator>().SetTrigger("die");
            if(coll.gameObject.GetComponent<Animator>()) {
                coll.gameObject.GetComponent<Animator>().SetTrigger("hit");
            }
            if (GetComponentInParent<BallHolder>().player.balls == 0)
            {
                GetComponentInParent<BallHolder>().player.GameOver();
            }
            */
        }
    
            //		GetComponent<Rigidbody2D>().freezeRotation = true;
            timeSinceLastBump = 0;
            gameObject.GetComponent<AudioSource>().Play();
    }
    void CheckFever(int amount = 0)
    {
        int speedState = GetComponentInParent<BallHolder>().increaseMultiplier(amount);
        if (speedState > 0)
        {
            force += 1;
            if (speedState == 2)
            {
                //EVENT #3 - FEVER REACHED

                GetComponentInParent<BallHolder>().FeverReached();
                Debug.Log("Fever Reached!");
                GetComponent<Animator>().SetTrigger("fever");

                //                    GetComponentInParent<LevelSound>().MaxMode();

            }
        }

    }
    public void Shrink()
    {
        gameObject.transform.localScale = gameObject.transform.localScale * .5f;
            //new Vector3(.1f, .1f, .1f);
    }

    public void Enlarge()
    {
        gameObject.transform.localScale = gameObject.transform.localScale * 2f;

    }
}
