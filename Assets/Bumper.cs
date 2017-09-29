using UnityEngine;
using System.Collections;

[System.Serializable]

public struct BumperSet
{
	public Sprite litUp;
	public Sprite destroyed;
}


public class Bumper : MonoBehaviour {
	private int timesHit = 0;
	public int requiredHits = 3;
	public BumperSet[] sprites;
	private float lightUpTime;
	private bool litUp = false;
	private Vector3 rotationNext;
	private Vector3 rotationPrevious;

	// Use this for initialization
	void Start () {
		rotationPrevious = new Vector3 (0, 0, 0);
		rotationNext = new Vector3(0,0, 0);

	}

	void OnDestroy() {
		if (GetComponentInParent<Level> ()) {
			GetComponentInParent<Level> ().ScanForCompletion ();
		}
	}
	
	// Update is called once per frame
	void Update () {
//		gameObject.transform.Rotate (Vector3.Lerp (rotationPrevious, rotationNext, Time.deltaTime));
		if (Time.time > lightUpTime && litUp) {
			litUp = false;
			if (timesHit < sprites.Length) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = sprites [timesHit].destroyed;
			}
			timesHit++;
			if (timesHit >= requiredHits) {
				GetComponent<Rigidbody2D> ().isKinematic = false;
				GetComponent<Animator> ().SetBool ("lastBump", true);		
			}
		}
	}


	public void Crumble() {

		Destroy(this.gameObject);

	}

	public void SwitchOff() {
	
		if (timesHit < sprites.Length) {
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[timesHit].destroyed;

		}
		/*
		if(timesHit > 0) {
			if(timesHit < sprites.Length) {
				gameObject.GetComponent<SpriteRenderer>().sprite = sprites[timesHit-1];
			}
		}
		*/
	}

	public void LightUp() {
		rotationPrevious = new Vector3 (0, 0, gameObject.transform.rotation.z);
		rotationNext = new Vector3(0,0, Random.Range(-10,10));
		if (timesHit < sprites.Length) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = sprites [timesHit].litUp;
		}
		litUp = true;
		lightUpTime = Time.time + .2f;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
//		GetComponent<Animator>().ForceStateNormalizedTime(0.0f);
//		GetComponent<Animator>().SetTrigger("bumped");	

	}
}
