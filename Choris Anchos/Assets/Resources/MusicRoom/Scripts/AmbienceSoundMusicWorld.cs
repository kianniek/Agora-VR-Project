using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSoundMusicWorld : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter musicSource;

    [SerializeField] private GameObject playerReference;
    [SerializeField] private GameObject handpanReference;
    [SerializeField] [Min(0)] private float stopPlayingDistances = 0.5f;
    [Tooltip("volume is mesured in db (Decibels). When 0 the audio is played at normal volume")]
    [SerializeField] [Range(-80, 10)]private float volume = 0;
    private string volumeParameter = "Volume";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(playerReference.transform.position, handpanReference.transform.position);

        musicSource.OverrideMaxDistance = musicSource.OverrideMinDistance = distance;
        float modefiedVolume = distance < 1 ? volume - distance : volume;
        modefiedVolume = Mathf.Clamp(modefiedVolume, -80, volume);
        musicSource.SetParameter(volumeParameter, modefiedVolume);
        bool stopMusic = distance < stopPlayingDistances;

        if (stopMusic)
        {
            musicSource.Stop();
        }

        if (musicSource != null && !musicSource.IsPlaying() && !stopMusic)
        {
            musicSource.Play();
        }
    }
}
