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
    public GameObject[] layers;
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
        layers = new GameObject[GetComponentsInChildren<Collider2D>().Length];
        for(int i = 0; i < GetComponentsInChildren<Collider2D>().Length; i++)
        {
            layers[i] = GetComponentsInChildren<Collider2D>()[i].gameObject;
        }
    }

    void OnDestroy() {
        if(transform.parent.GetComponent<ProceduralLevel>())
        {
            transform.parent.GetComponent<ProceduralLevel>().lastEnergyCollectionPosition = transform;

        }
        else
        {
            Debug.Log("Can't find level during energy destruction");
        }

    }
	
	// Update is called once per frame
	void Update () {
        if(scaleUp)
        {
            
            transform.localScale = Vector3.Slerp(transform.localScale, originalScale, .01f);
            if(transform.localScale == originalScale)
            {
                scaleUp = false;
            }
        }
      
	}


    public bool isDead()
    {
        GetComponent<AudioSource>().PlayOneShot(hit);

        //HIT

        if (layers.Length > 0)
        {
            if (layers.Length > timesHit)
            {
                layers[timesHit].SetActive(false);
            }
        }
        timesHit++;

        if (timesHit > layers.Length - 1)
        {
            Instantiate(explosion, transform.position, Quaternion.identity, transform.parent);
            return true;
        }
        else
        {
            return false;
        }


    }
}
