using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSoundYogaRoom : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The StudioEventEmitter component for the music source.")]
    private StudioEventEmitter musicSource;

    [SerializeField]
    [Tooltip("The GameObject representing the player.")]
    private GameObject playerReference;

    [SerializeField]
    [Range(-80, 10)]
    [Tooltip("The volume of the music in dB (Decibels). When set to 0, the audio is played at normal volume.")]
    private float volume = 0;

    private readonly string volumeParameter = "Volume";

    private float followSpeed = 5f; // Adjust this value to control the smoothness of the follow.

    private void Update()
    {
        FollowObject(playerReference);
        StartStopMusic();
    }

    private void FollowObject(GameObject obj)
    {
        // Get the target position to follow
        Vector3 targetPosition = obj.transform.position;

        // Smoothly interpolate towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    private void StartStopMusic()
    {
        // Check if the music source is assigned
        if (musicSource == null)
            return;

        // Set the distance parameters for sound attenuation
        musicSource.OverrideMinDistance = 10;
        musicSource.OverrideMaxDistance = musicSource.OverrideMinDistance + 10;

        // Set the volume parameter for the music source
        musicSource.SetParameter(volumeParameter, volume);
        
        if (!musicSource.IsPlaying())
        {
            musicSource.Play();
        }
    }
}
