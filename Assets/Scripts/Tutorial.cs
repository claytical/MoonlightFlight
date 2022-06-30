using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public struct TutorItem
{
    public string text;
    public string animationTrigger;
    public GameObject objectToAnimate;
}

public class Tutorial : MonoBehaviour
{
    public Text computer;
    public TutorItem[] messages;
    public ProceduralLevel level;
    public float timeBetweenMesssages;
    private float timeUntilNextMessage;
    private int messageIndex = 0;
    private Breakable[] energy;
    // Start is called before the first frame update

    void Start()
    {
        energy = level.GetComponentsInChildren<Breakable>();
        
        for(int i = 0; i < energy.Length; i++)
        {
            Debug.Log("ENERGY SOURCE #:" + i);
            for(int j = 0; j < messages.Length; j++)
            {
                messages[j].objectToAnimate = energy[i].gameObject;
                Debug.Log("MESSAGE #:" + j);

            }
        }

        Step();

    }

    private bool Step()
    {
        if (messages.Length > 0 && messageIndex < messages.Length)
        {
            computer.text = messages[messageIndex].text;
            if (messages[messageIndex].animationTrigger != "")
            {
                for (int i = 0; i < energy.Length; i++)
                {
                    if(energy[i])
                    {
                        if(energy[i].GetComponent<Animator>())
                        {
                            energy[i].GetComponent<Animator>().SetTrigger(messages[messageIndex].animationTrigger);

                        }
                        else
                        {
                            //hack to remove tutorial panel during testing
                            gameObject.SetActive(false);
                        }
                    }
                }
            }
            timeUntilNextMessage = Time.time + timeBetweenMesssages;
            return true;
        }
        else
        {
            gameObject.SetActive(false);
            return false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= timeUntilNextMessage)
        {
            messageIndex++;

            if (!Step())
            {
                gameObject.SetActive(false);

            }
        }        
    }
}
