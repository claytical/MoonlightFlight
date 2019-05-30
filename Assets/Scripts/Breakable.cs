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
	public AudioClip hit;
    public AudioClip finished;
    public Flys flies;
    private GameObject ball;
	private float lightUpTime;
	private bool litUp = false;
	private bool isDying = false;

	// Use this for initialization
	void Start () {
	}

	void OnDestroy() {
		if (GetComponentInParent<Level> ()) {
			GetComponentInParent<Level> ().ScanForCompletion ();
            Debug.Log("Scanning for Completion");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > lightUpTime && litUp) {
			litUp = false;
		}

        int spritesDeactivated = 0;

        for (int i = 0; i < sprites.Length; i++)
        {
            if(!sprites[i].activeSelf)
            {
                spritesDeactivated++;
            }
        }
        if(spritesDeactivated == sprites.Length)
        {
            //set flies free first
            flies.GetComponent<Transform>().SetParent(gameObject.transform.parent);
            flies.Free(ball);
            Destroy(this.gameObject,1);
        }
	}


	public void Crumble() {
		if (!isDying) {
			Destroy (this.gameObject, 1f);
		}
		isDying = true;
	}

	public void SwitchOff() {
	
	}

	public void LightUp(GameObject b) {
        //HIT
        ball = b;
        if (timesHit < sprites.Length - 1 ) {
            sprites[timesHit].SetActive(false);
			GetComponentInParent<AudioSource> ().PlayOneShot (hit);
		} else {
            GetComponentInParent<AudioSource>().PlayOneShot(finished);
            sprites[timesHit].SetActive(false);
            GetComponentInParent<Level>().AddFliesReleased(gameObject.GetComponentsInChildren<Fly>().Length, b.GetComponentInParent<BallHolder>().player.inkAmount, b.GetComponentInParent<BallHolder>().multiplier);
            GetComponent<Rigidbody2D> ().isKinematic = false;

		}
		timesHit++;
	}
}
