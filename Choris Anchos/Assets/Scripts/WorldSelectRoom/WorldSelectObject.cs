using FMOD;
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
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 100f;

    Rigidbody rb;

    void Start()
    {
        instance = RuntimeManager.CreateInstance(WorldMusic);

        // Setting 3D attributes
        ATTRIBUTES_3D attr = new ATTRIBUTES_3D();
        RuntimeManager.StudioSystem.getListenerAttributes(0, out attr);
        attr.position = RuntimeUtils.ToFMODVector(this.transform.position);
        attr.forward = RuntimeUtils.ToFMODVector(this.transform.forward);
        attr.up = RuntimeUtils.ToFMODVector(this.transform.up);
        attr.velocity = RuntimeUtils.ToFMODVector(rb ? rb.velocity : Vector3.zero);
        instance.set3DAttributes(attr);

        // Setting min and max distance for the instance
        instance.setProperty(EVENT_PROPERTY.MINIMUM_DISTANCE, minDistance);
        instance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, maxDistance);

        instance.start();
        rb = GetComponent<Rigidbody>();
        RuntimeManager.AttachInstanceToGameObject(instance, this.transform, rb);
    }

    private void Update()
    {
        // Setting min and max distance for the instance
        instance.setProperty(EVENT_PROPERTY.MINIMUM_DISTANCE, minDistance);
        instance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, maxDistance);
    }
    private void OnDisable()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, minDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, maxDistance);
    }
}
