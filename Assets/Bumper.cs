using UnityEngine;
using System.Collections;

public class Bumper : MonoBehaviour {
	private int timesHit = 0;
	public int requiredHits = 3;
	public Sprite litUpSprite;
	public Sprite[] deterioratedSprite;
	private Sprite originalSprite;

	// Use this for initialization
	void Start () {
		originalSprite = GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Crumble() {

		transform.parent.parent.parent.GetComponent<LevelGenerator>().removeBumper();
		Destroy(this.gameObject);
	}

	public void SwitchOff() {
		if(timesHit > 0) {
			if(timesHit - 1 < deterioratedSprite.Length) {
				gameObject.GetComponent<SpriteRenderer>().sprite = deterioratedSprite[timesHit-1];
			}
		}
		else {
//			gameObject.GetComponent<SpriteRenderer>().sprite = originalSprite;
		}
	}

	public bool LightUp() {
		gameObject.GetComponent<SpriteRenderer>().sprite = litUpSprite;
		timesHit++;
//		GetComponent<Animator>().SetInteger("bumps", timesHit);
		GetComponent<Animator>().ForceStateNormalizedTime(0.0f);
		GetComponent<Animator>().SetTrigger("bumped");	

		if (timesHit >= requiredHits) {
			GetComponent<Rigidbody2D>().isKinematic = false;
			GetComponent<Animator>().SetBool("lastBump", true);		
			return true;
		}
		else {
			return false;
		}
	}
}
