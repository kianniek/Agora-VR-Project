using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource[] audioSources;
    public AudioClip audioClip;

    void Start()
    {
        // Get a reference to each AudioSource component
        audioSources = GetComponents<AudioSource>();
    }

    public void PlayAudio(float pitch)
    {
        // Find an available AudioSource component and play the audio clip
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.pitch = pitch;
                audioSource.clip = audioClip;
                audioSource.Play();
                break;
            }
        }
    }
}
