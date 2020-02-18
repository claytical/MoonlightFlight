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
    public bool shrinksIntruder;
    public bool enlargesIntruder;
    public GameObject[] sprites;
    public GameObject explosion;
	public AudioClip hit;
    public Flys flies;
    private GameObject ball;
	private float lightUpTime;
	private bool litUp = false; //
	private bool isDying = false;

	// Use this for initialization
	void Start () {
	}

	void OnDestroy() {
        /*
        if (GetComponentInParent<Level> ()) {
			GetComponentInParent<Level> ().ScanForCompletion ();
            Debug.Log("Scanning for Completion");
		}
	*/
    }
	
	// Update is called once per frame
	void Update () {
        /*if (Time.time > lightUpTime && litUp) {
			litUp = false;
		}*/
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
            if (shrinksIntruder)
            {
                ball.GetComponent<Ball>().Shrink();
            }
            if(enlargesIntruder)
            {
                ball.GetComponent<Ball>().Enlarge();

            }
/*            flies.GetComponent<Transform>().SetParent(gameObject.transform.parent);
            flies.Free(ball);
  */
            Destroy(this.gameObject);
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

	public bool LightUp(GameObject b) {
        GetComponent<AudioSource>().PlayOneShot(hit);
        //HIT
        ball = b;
        Debug.Log("Creating Explosion");
        Instantiate(explosion, transform.position, Quaternion.identity, transform.parent);
        //
        if (timesHit <= sprites.Length - 1 ) {
            //activate particles
          //    particles[timesHit].Play();d
            sprites[timesHit].SetActive(false);
            timesHit++;
            return true;
        }
/*
        else {
            sprites[timesHit].SetActive(false);
            GetComponentInParent<EndlessLevel>().AddFliesReleased(gameObject.GetComponentsInChildren<Fly>().Length, b.GetComponentInParent<BallHolder>().player.inkAmount, b.GetComponentInParent<BallHolder>().multiplier);            
            GetComponent<Rigidbody2D> ().isKinematic = false;
            timesHit++;

        }
  */
    return false;
    }
}
