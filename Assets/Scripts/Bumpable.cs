using UnityEngine;
using System.Collections;

[System.Serializable]

public struct BumpableSet
{
	public Sprite frame;
	public Sprite bumpedFace;
	public Sprite idleFace;
}


public class Bumpable : MonoBehaviour {
	private int timesHit = 0;
//	public int requiredHits = 3;
	public BumpableSet[] sprites;
	public SpriteRenderer color;
	public SpriteRenderer tint;
	public SpriteRenderer frame;
	private SpriteRenderer face;
	private float lightUpTime;
	private bool litUp = false;

	// Use this for initialization
	void Start () {
		face = GetComponent<SpriteRenderer> ();
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
				face.sprite = sprites [timesHit].idleFace;
				//				gameObject.GetComponent<SpriteRenderer> ().sprite = sprites [timesHit].destroyed;
			}
			timesHit++;
			if (timesHit >= sprites.Length + 1) {
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
//			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[timesHit].destroyed;
			face.sprite = sprites [timesHit].idleFace;

		}
		/*
		if(timesHit > 0) {
			if(timesHit < sprites.Length) {
				face.sprite = sprites [timesHit-1].idleFace;
			}
		}
		*/
	}

	public void LightUp() {
		transform.Translate (Random.Range (-.01f, .01f), Random.Range (-.01f, .01f), 0);

		GetComponent<Animator>().SetTrigger("bumped");	
		if (timesHit < sprites.Length) {
			face.sprite = sprites [timesHit].bumpedFace;
			frame.sprite = sprites [timesHit].frame;
		}
		litUp = true;
		lightUpTime = Time.time + .2f;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
//		GetComponent<Animator>().ForceStateNormalizedTime(0.0f);

	}
}
