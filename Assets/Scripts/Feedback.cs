using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feedback : MonoBehaviour
{
    public Text message;
    // Start is called before the first frame update
    void Start()
    {
    }


    public void TurnOff()
    {
        gameObject.SetActive(false);

    }

    public void SetMessage(string m)
    {
        message.text = m;
        Invoke("TurnOff", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
