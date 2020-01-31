using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public GameObject spawnLocations;
    public GameObject platforms;
    public GameObject[] breakables;
    public GameObject powerUp;
    public ProceduralSet currentSet;
    public int numberOfObjectsToPlace;
    public int sets = 5;
    public AudioSource fullEnergy;
    public int energyRequiredForPowerUp = 25;
    //remaining spaces to populate

    // Start is called before the first frame update
    void Start()
    {
        platforms.SetActive(true);
    }

    public void LowEnergy()
    {
        currentSet.LowEnergy();
    }

    public void PlatformTransparency(bool transparent)
    {
        Rigidbody2D [] bumpables = platforms.GetComponentsInChildren<Rigidbody2D>();

            for(int i = 0; i < bumpables.Length; i++)
            {
                if(bumpables[i].GetComponent<Animator>()) {
                    if (transparent)
                    {
                        bumpables[i].gameObject.GetComponent<Animator>().SetTrigger("transparent");
                    }
                    else
                    {
                        bumpables[i].gameObject.GetComponent<Animator>().SetTrigger("solid");
                    }
                if(bumpables[i].GetComponent<BoxCollider2D>())
                {
                    bumpables[i].GetComponent<BoxCollider2D>().enabled = !transparent;
                }
            }
        }
    }

    public void PowerUp()
    {
//        Instantiate(powerUp, transform);
        fullEnergy.Play();
        Debug.Log("POWER UP!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Setup()
    {
        //transition to first part of new audiomixerx
        //turn on platformsx
        //generate new layout based on spawnlocations

    }
}
