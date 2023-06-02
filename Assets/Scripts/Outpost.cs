using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PixelCrushers.DialogueSystem;

public class Outpost : MonoBehaviour
{
    public List<GameObject> inTheWay;
    public string conversation;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartConversation()
    {
        DialogueManager.StartConversation(conversation);
       
    }
    private void OnEnable()
    {
            for (int i = 0; i < inTheWay.Count; i++)
            {
                inTheWay[i].SetActive(false);
            }
        
    }
    private void OnDisable()
    {
        //NULL on self destruct?
        if(!(inTheWay is null))
        {
            for (int i = 0; i < inTheWay.Count; i++)
            {
                inTheWay[i].SetActive(true);
            }

        }

    }

    private void OnDestroy()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
