using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicButton : MonoBehaviour
{
    public string Preference;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(PlayerPrefs.HasKey(Preference));

    }

}
