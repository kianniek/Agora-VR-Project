using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelectObject : MonoBehaviour
{
    private EventInstance instance;
    [SerializeField] private EventReference WorldMusic;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        instance = RuntimeManager.CreateInstance(WorldMusic);
        instance.start();
        rb = GetComponent<Rigidbody>();
        RuntimeManager.AttachInstanceToGameObject(instance, this.transform, rb);
    }

    private void OnDisable()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

}
