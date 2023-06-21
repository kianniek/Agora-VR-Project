using FMODUnity;
using UnityEngine;

public class AmbienceSoundMusicWorld : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The StudioEventEmitter component for the music source.")]
    private StudioEventEmitter musicSource;

    [SerializeField]
    [Tooltip("The GameObject representing the player.")]
    private GameObject playerReference;

    [SerializeField]
    [Tooltip("The GameObject representing the handpan.")]
    private GameObject handpanReference;

    [SerializeField]
    [Min(0)]
    [Tooltip("The distance at which the music should stop playing.")]
    private float stopPlayingDistance = 1.5f;

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

        // Calculate the distance between the player and handpan reference
        float distance = Vector3.Distance(playerReference.transform.position, handpanReference.transform.position);

        // Set the distance parameters for sound attenuation
        musicSource.OverrideMinDistance = distance;
        musicSource.OverrideMaxDistance = musicSource.OverrideMinDistance + 10;

        // Set the volume parameter for the music source
        musicSource.SetParameter(volumeParameter, volume);

        // Determine if the music should be stopped based on the distance
        bool stopMusic = distance < stopPlayingDistance;

        // Stop the music if it is playing and should be stopped
        if (stopMusic && musicSource.IsPlaying())
        {
            musicSource.Stop();
        }
        // Start playing the music if it is not playing and should be played
        else if (!musicSource.IsPlaying())
        {
            musicSource.Play();
        }
    }
}
