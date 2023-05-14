using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShot3D : MonoBehaviour
{
    [SerializeField] EventReference sound;  // Reference to the audio event to be played

    public void PlayOneShot()
    {
        AudioManager.instance.PlayOneShot3D(sound, this.transform.position);
    }
}
