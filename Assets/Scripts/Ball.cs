using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	private int timeSinceLastBump = 0;
	private bool isDead = false;
    private bool isWarping = false;
    public bool inPlay = false;
	private Vector3 warpPosition;
	private Vector3 originalPosition;
    public float force;
	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastBump++;
        //              GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.acceleration.x * force, Input.acceleration.y * force));
        if (inPlay)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Input.acceleration.x, Input.acceleration.y) * force;
        }  
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
		Debug.Log ("Warping!");
		warp (other.gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll) {
			gameObject.GetComponent<AudioSource> ().Play ();
        Debug.Log("This: " + gameObject.tag + " Other: " + coll.gameObject.tag);
		if(coll.gameObject.tag == "Boundary") {
			isDead = true;
            GetComponentInParent<BallHolder>().player.GameOver();
            GetComponent<Animator>().SetTrigger("die");
            /*
            if (GetComponentInParent<BallHolder> () != null) {
				if (GetComponentInParent<BallHolder> ().player.balls == 0) {
					GetComponentInParent<BallHolder> ().player.GameOver ();
				}
			}
            */

		}
		if (coll.gameObject.tag == "Avoid") {
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
        if (coll.gameObject.tag == "Disappearing") {

//			coll.gameObject.GetComponent<Bumpable> ().LightUp ();
            coll.gameObject.GetComponent<Breakable>().LightUp(this.gameObject);
            
		}
		if(coll.gameObject.tag == "Tone") {
			coll.gameObject.GetComponent<Point>().Hit(coll.relativeVelocity.magnitude);
		}
		if (coll.gameObject.tag == "Bumpable") {
            //Check for polygon shenanigans

			coll.gameObject.GetComponent<Immovable> ().LightUp ();
			GetComponentInParent<BallHolder>().addPoints(1);
		}
//		GetComponent<Rigidbody2D>().freezeRotation = true;
		timeSinceLastBump = 0;
	}
}
