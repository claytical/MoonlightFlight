using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayLoopingSound(AudioClip clip, float volume = 1f)
    {
        AudioSource source = GetComponent<AudioSource>();
        if (source != null && clip != null)
        {
            source.clip = clip;
            source.volume = volume;
            source.loop = true;
            source.Play();
        }
    }

    public void StopSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void AdjustPitch(float pitch)
    {
        if (audioSource != null)
        {
            audioSource.pitch = Mathf.Clamp(pitch, -4f, 4f);
        }
    }
}
