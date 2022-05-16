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
    private GameObject[] layers;
    public GameObject explosion;
	public AudioClip hit;
	private float lightUpTime;
	private bool isDying = false;
    private bool scaleUp = true;
    private Vector3 originalScale;

	// Use this for initialization
	void Start () {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        layers = new GameObject[GetComponentsInChildren<SpriteRenderer>().Length];
        for(int i = 0; i < GetComponentsInChildren<SpriteRenderer>().Length; i++)
        {
            layers[i] = GetComponentsInChildren<SpriteRenderer>()[i].gameObject;
        }
    }

    void OnDestroy() {

    }
	
	// Update is called once per frame
	void Update () {
        if(scaleUp)
        {
            
            transform.localScale = Vector3.Slerp(transform.localScale, originalScale, .001f);
            if(transform.localScale == originalScale)
            {
                scaleUp = false;
            }
        }

        int spritesDeactivated = 0;



        for (int i = 0; i < layers.Length; i++)
        {
            if(!layers[i].activeSelf)
            {
                spritesDeactivated++;
            }
        }
        if(spritesDeactivated == layers.Length)
        {

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
        Instantiate(explosion, transform.position, Quaternion.identity, transform.parent);
        //0 <= 2 - 1
        //0 <= 1

        // 1 <= 2 - 1

        // 1 < 1
        layers[timesHit].SetActive(false);

        if (timesHit < layers.Length - 1 ) {
            Debug.Log("TIMES HIT: " + timesHit + " LAYERS LENGTH: " + layers.Length);
            timesHit++;
            return true;
        }
    return false;
    }
}
