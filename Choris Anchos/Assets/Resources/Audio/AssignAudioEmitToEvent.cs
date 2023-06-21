using FMODUnity;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[ExecuteInEditMode]
public class AssignAudioEmitToEvent : MonoBehaviour
{
    [InspectorButton("Running")]
    public bool Run;

    HVRCollisionEvents collisionEvents;
    StudioEventEmitter emitter;
    // Start is called before the first frame update
    void Running()
    {
        collisionEvents = GetComponent<HVRCollisionEvents>();
        if (collisionEvents == null)
        {
            Debug.Log("HVRCollisionEvents is null");
            return;
        }

        emitter = GetComponent<StudioEventEmitter>();
        if (emitter == null)
        {
            Debug.Log("StudioEventEmitter is null");
            return;
        }

        UnityAction action = () =>
        {
            Debug.Log("Playing emitter");
            emitter.Play();
        };

        collisionEvents.ThresholdMet.AddListener(action);
    }
}
