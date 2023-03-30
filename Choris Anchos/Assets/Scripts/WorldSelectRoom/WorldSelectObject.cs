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
    }

    // Update is called once per frame
    void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(instance, this.transform, rb);
    }

}
