using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProceduralMusic : MonoBehaviour
{

    public int loopIndex = 0;
    private int previousLoopIndex;
    public AudioMixerSnapshot track;
    public List<AudioSource> loops;
    private bool transitioning = false;
    private bool playedTransitionSoundEffect = false;

    public GameObject particles;


    public float shake;
    public float shakeAmount = .7f;
    public float decreaseFactor = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        loops = new List<AudioSource>();
        Debug.Log(GetComponentsInChildren<Transform>().Length + " possible loops found");
        for (int i = 0; i < GetComponentsInChildren<Transform>().Length; i++)
        {
            AudioSource loop = GetComponentsInChildren<Transform>()[i].GetComponent<AudioSource>();
            if(loop != null && !GetComponentsInChildren<Transform>()[i].GetComponent<ProceduralMusic>())
            {
                loops.Add(loop);

            }
        }
        track.TransitionTo(1f);
        if(loops.Count > 0)
        {
            Play();
        }
    }

    public void Play()
    {
        Debug.Log("Playing #" + loopIndex);
        loops[loopIndex].loop = true;
        loops[loopIndex].Play();

        //force previous tracks to stop
        for(int i = 0; i < loopIndex; i++)
        {
            if (loops[i].isPlaying)
            {
                //throw in effect in case it's glitching

                GetComponent<AudioSource>().Play();
                loops[i].Stop();
            }
        }

        //force all upcoming tracks to stop if they are playing
        for (int i = loopIndex + 1; i < loops.Count; i++)
        {
            if (loops[i].isPlaying)
            {
                GetComponent<AudioSource>().Play();
                loops[i].Stop();
            }
        }
    }

    public void ChangeTrack()
    {
        //track 0 turns off looping
        //track 1 turns off looping
        loops[loopIndex].loop = false;
        //previousLoopIndex becomes 0
        //previousLoopIndex becomes 1
        previousLoopIndex = loopIndex;
        //loopIndex becomes 1
        //loopIndex becomes 2
        loopIndex++;
        if(loopIndex > loops.Count - 1)
        {
            loopIndex = 0;
        }
        playedTransitionSoundEffect = false;
        transitioning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(shake > 0)
        {
            Vector3 newCameraPosition = Random.insideUnitSphere * shakeAmount;
            newCameraPosition.z = Camera.main.transform.localPosition.z;
            Camera.main.transform.localPosition = newCameraPosition;
            shake -= .01f * decreaseFactor;
        }
        else
        {
            shake = 0;
        }
        if(transitioning)
        {
            if(!loops[previousLoopIndex].isPlaying) {
                Debug.Log("STARTING NEW TRACK! #" + loopIndex);
                //track 1 stopped (previousLoopIndex = 0)
                //trying to advance to track 2 (track 1 has loop set to false) 
                //changes again, now has track 3 to look for

                //should skip track 2 and advance to track 3


                Play();
                transitioning = false;
            }
            else
            {

                if((loops[previousLoopIndex].time > loops[previousLoopIndex].clip.length - 1 && !playedTransitionSoundEffect))
                {

                    if(loops[previousLoopIndex].time > loops[previousLoopIndex].clip.length - 1 && !playedTransitionSoundEffect)
                    {
                        Debug.Log("Long Clip, Playing Transition Effect");
                    }
                    if(loops[previousLoopIndex].clip.length <= 3 && !playedTransitionSoundEffect)
                    {
                        Debug.Log("SHort Clip, just playing the sound effect");
                    }
                    //ascending swish effect is 3 seconds, so the clip should play 2 seconds into the next lo
                    playedTransitionSoundEffect = true;
                    //play transition

                    //instantiate particle warp
                    GameObject warp = Instantiate(particles, transform.position, transform.rotation);
                    GetComponent<AudioSource>().Play();
                    shake = 1f;

                }
                //   Debug.Log("PREVIOUS TRACK STILL PLAYING");
            }
        }

    }
}
