using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipLock : MonoBehaviour
{
    public int seedsRequired;
    public Sprite lockImage;
    public Button play;
    public Text additionalSeedsRequired;

    // Start is called before the first frame update
    void Start()
    {
    }

    public bool SetLock()
    {
        int seedsCollected = PlayerPrefs.GetInt("seeds", 0);
        if (seedsCollected >= seedsRequired)
        {
            play.interactable = true;
            additionalSeedsRequired.gameObject.SetActive(false);
            return true;
        }
        else
        {
            GetComponentInChildren<Image>().sprite = lockImage;
            GetComponentInChildren<Image>().SetNativeSize();
            GetComponentInChildren<Animator>().enabled = false;
            play.interactable = false;
            additionalSeedsRequired.gameObject.SetActive(true);
            additionalSeedsRequired.text = (seedsRequired - seedsCollected).ToString("0 More Seeds Required");
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
