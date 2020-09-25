using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationContol : MonoBehaviour
{

    private Animator[] animations;
    public string[] clips;
//    public int clip;
    // Start is called before the first frame update
    void OnEnable()
    {
        animations = GetComponentsInChildren<Animator>();
        for (int i = 0; i < animations.Length; i++) 
        {
            animations[i].Play(clips[i], -1, 0);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
