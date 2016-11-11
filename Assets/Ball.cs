using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public Material death;
	public Material bump;

	private int timeSinceLastBump = 0;
	private bool isDead = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastBump++;

		if(isDead) {
			if(!GetComponentInChildren<ParticleSystem>().IsAlive()) {
				Destroy(gameObject);
			}
		}
		else 	if(!GetComponentInChildren<ParticleSystem>().IsAlive()) {
			GetComponent<Rigidbody2D>().freezeRotation = false;

		}		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.tag == "Boundary") {
			isDead = true;
			GetComponent<Animator>().SetTrigger("die");

		}
		if (coll.gameObject.tag == "Avoid") {
			coll.gameObject.GetComponent<AudioSource>().Play();
			//Trigger ball animation
			isDead = true;
			GetComponent<Animator>().SetTrigger("die");
			if(coll.gameObject.GetComponent<Animator>()) {
				coll.gameObject.GetComponent<Animator>().SetTrigger("hit");
			}
			GetComponentInChildren<ParticleSystemRenderer>().material = death;
			GetComponentInChildren<ParticleSystem>().Play();
		}
		if (coll.gameObject.tag == "Disappearing") {

			if(!isDead) {
				GetComponentInChildren<ParticleSystemRenderer>().material = bump;
				GetComponentInChildren<ParticleSystem>().Emit(timeSinceLastBump/10);
			}

			if(coll.gameObject.GetComponent<Bumper>().LightUp()) {
				if(GetComponentInParent<BallHolder>().player.tutor) {
					GetComponentInParent<BallHolder>().player.tutor.Continue(3);
				}
			}

			GetComponentInParent<BallHolder>().addPoints(timeSinceLastBump * 2);

		}
		if(coll.gameObject.tag == "Tone") {
			coll.gameObject.GetComponent<Point>().Hit(coll.relativeVelocity.magnitude);
/*			float p = coll.gameObject.transform.position.y + 4;
			float v = Mathf.Clamp(coll.relativeVelocity.magnitude, 0, .75f);
			coll.gameObject.GetComponent<AudioSource>().pitch = Mathf.Clamp(p, -1, 3);
			coll.gameObject.GetComponent<AudioSource>().volume = v;
			coll.gameObject.GetComponent<AudioSource>().Play();
*/
		if(gameObject.GetComponentInParent<BallHolder>().player.tutor) {
				Debug.Log("Bumped against line move tutorial forward");
				gameObject.GetComponentInParent<BallHolder>().player.tutor.Continue(2);
			}
		}
		if (coll.gameObject.tag == "Bumpable") {

			if(!isDead) {
				GetComponentInChildren<ParticleSystemRenderer>().material = bump;
				GetComponentInChildren<ParticleSystem>().Emit(timeSinceLastBump/10);			
			}
			GetComponentInParent<BallHolder>().addPoints(timeSinceLastBump);
		}
		GetComponent<Rigidbody2D>().freezeRotation = true;
		timeSinceLastBump = 0;
	}
}
