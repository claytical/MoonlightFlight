using UnityEngine;
using System.Collections;

[System.Serializable]

public struct BreakableSet
{
	public GameObject top;
	public GameObject middle;
	public GameObject bottom;
}


public class Breakable : MonoBehaviour {
	private int timesHit = 0;
//	public int requiredHits = 3;
//	public BreakableSet[] sprites;
    public GameObject[] sprites;
	public SpriteRenderer color;
	public SpriteRenderer tint;
	public SpriteRenderer frame;
	public AudioClip[] bumpFx;
	public AudioClip[] deathFx;
	public AudioClip hit;
	public GameObject explosion;
	private SpriteRenderer face;
	private float lightUpTime;
	private bool litUp = false;
	private bool isDying = false;

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
		if (Time.time > lightUpTime && litUp) {
			litUp = false;
			if (timesHit < sprites.Length) {
//				face.sprite = sprites [timesHit].idleFace;
			}
		}
	}


	public void Crumble() {
		if (!isDying) {
//			GetComponent<ParticleSystem> ().Play ();
			GetComponent<AudioSource> ().PlayOneShot (deathFx [Random.Range (0, deathFx.Length)]);

//			GetComponent<ParticleSystem> ().Play ();
			Destroy (this.gameObject, 1f);
		}
		isDying = true;
	}

	public void SwitchOff() {
	
//		if (timesHit < sprites.Length) {
//			face.sprite = sprites [timesHit].idleFace;
//		}
	}

	public void LightUp() {
        //HIT
		if (timesHit < sprites.Length - 1) {
            sprites[timesHit].SetActive(false);
			//GetComponent<Animator>().SetTrigger("bumped");
            //			face.sprite = sprites [timesHit].bumpedFace;
            //			frame.sprite = sprites [timesHit].frame;
            //			transform.Translate (Random.Range (-.01f, .01f), Random.Range (-.01f, .01f), 0);		
			GetComponentInParent<AudioSource>().Play();
			GetComponentInParent<AudioSource> ().PlayOneShot (hit);
/*			litUp = true;
			lightUpTime = Time.time + .2f;
            */
		} else {
            
			Instantiate (explosion, transform.position, transform.rotation);
			GetComponent<Rigidbody2D> ().isKinematic = false;
			GetComponent<Animator> ().SetBool ("lastBump", true);		

		}
		timesHit++;
	}
}
