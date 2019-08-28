using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    int timesHit = 0;
    public int maxHits;
    public SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Bumped(GameObject b)
    {
        //HIT
        Debug.Log("Changing Color");
        if (timesHit < maxHits)
        {
            //           GetComponentInParent<AudioSource>().PlayOneShot(hit);
            Color c = new Color();
            c = Random.ColorHSV();
            sprite.color = c;
        }
        else
        {
            Destroy(gameObject);
            //         GetComponentInParent<AudioSource>().PlayOneShot(finished);
   //         sprites[timesHit].SetActive(false);
   //         GetComponentInParent<Level>().AddFliesReleased(gameObject.GetComponentsInChildren<Fly>().Length, b.GetComponentInParent<BallHolder>().player.inkAmount, b.GetComponentInParent<BallHolder>().multiplier);
   //         GetComponent<Rigidbody2D>().isKinematic = false;

        }
        timesHit++;
        
    }

}
