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
    public float force;
    public GameObject fly;
	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
	}

    // Update is called once per frame

    void FixedUpdate()
    {
        if (inPlay && canTilt)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Input.acceleration.x, Input.acceleration.y) * force;
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

    void Update () {
		timeSinceLastBump++;
        //              GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.acceleration.x * force, Input.acceleration.y * force));
            //GetComponent<Rigidbody2D>().AddForce(, ForceMode2D.Impulse);
//        Vector3 newPosition = new Vector3();
  //      newPosition = transform.position + (Input.acceleration * .01f);

    //    transform.position = newPosition;


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
            GetComponentInParent<LevelSound>().NormalMode();
            GetComponentInParent<BallHolder>().player.GameOver();

        }
    }

	void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Disappearing") {

            //			coll.gameObject.GetComponent<Bumpable> ().LightUp ();
            coll.gameObject.GetComponent<Breakable>().LightUp(this.gameObject);
            
		}

        if (coll.gameObject.tag == "Bumpable") {
            //Check for polygon shenanigans
            frameCountAtBump = Time.frameCount + framesUntilTilt;
            coll.gameObject.GetComponent<Animator>().SetTrigger("hit");
            if (canTilt)
            {
                canTilt = false;
                GetComponent<Animator>().SetBool("tiltLocked", true);
            }
                coll.gameObject.GetComponent<Immovable> ().LightUp ();
            int speedState = GetComponentInParent<BallHolder>().increaseMultiplier();
            if (speedState > 0)
            {
                force += 1;
                if(speedState == 2)
                {
                    GetComponent<Animator>().SetTrigger("fever");
                    GetComponentInParent<LevelSound>().MaxMode();

                }
            }
        }
        /*
        if (coll.gameObject.tag == "Boundary")
        {
            isDead = true;
            GetComponentInParent<LevelSound>().NormalMode();
            GetComponentInParent<BallHolder>().player.GameOver();
     
        }
   */
        if (coll.gameObject.tag == "Avoid")
        {
            //			gameObject.GetComponentInParent<BallHolder> ().removePoints ();
            //			coll.gameObject.GetComponent<AudioSource>().Play();
            //Trigger ball animation
            isDead = true;
            GetComponentInParent<BallHolder>().player.GameOver();
            Debug.Log("Got hit with avoid");
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
}
