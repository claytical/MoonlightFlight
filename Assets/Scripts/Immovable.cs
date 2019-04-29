using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immovable : MonoBehaviour {
	private float lightUpTime;
	private bool currentlyLitUp;
//	public Sprite litUp;
//	private Sprite originalSprite;
	// Use this for initialization
	void Start () {
//		originalSprite = gameObject.GetComponent<SpriteRenderer> ().sprite;
	}
	
	// Update is called once per frame
	void Update () {
	/*	if (Time.time > lightUpTime && currentlyLitUp) {
			currentlyLitUp = false;
			gameObject.GetComponent<SpriteRenderer> ().sprite = originalSprite;
		}
        */
	}

	public void LightUp() {
        /*
        gameObject.GetComponent<SpriteRenderer> ().sprite = litUp;
		lightUpTime = Time.time + .2f;
		currentlyLitUp = true;
	*/
    }
}
