using HurricaneVR.Framework.Shared;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace HurricaneVR.Framework.Components
{
    /// <summary>
    /// Helper component used to propogate a collision if a force or velocity threshold was met.
    /// </summary>
    [ExecuteInEditMode]
    public class HVRCollisionEvents : MonoBehaviour
    {
        [Header("Settings")]

        public CollisionEventType CollisionType = CollisionEventType.Impulse;

        [Tooltip("Force threshold to breach to fire the ThresholdMet event")]
        public float ForceThreshold;

        [Tooltip("Collision velocity threshold to breach to fire the ThresholdMetEvent")]
        public float VelocityThreshold;

        public UnityEvent ThresholdMet;

        [Header("Debug")]
        public float LastImpulse;
        public float LastVelocity;

        public float MaxImpulse;
        public float MaxVelocity;

        [InspectorButton("AddEventDestruct")]
        public bool addEventDestruct;
        private void AddEventDestruct()
        {
            ThresholdMet.AddListener(GetComponent<HVRDestructible>().Destroy);
        }
        protected virtual void OnCollisionEnter(Collision other)
        {
            LastImpulse = other.impulse.magnitude;
            LastVelocity = other.relativeVelocity.magnitude;

            MaxImpulse = Mathf.Max(MaxImpulse, LastImpulse);
            MaxVelocity = Mathf.Max(MaxVelocity, LastVelocity);

            var forceMet = LastImpulse > ForceThreshold;
            var velocityMet = LastVelocity > VelocityThreshold;

            SendMessage("PlayOneShot");

            if (CollisionType == CollisionEventType.Impulse && forceMet ||
                CollisionType == CollisionEventType.Velocity && velocityMet ||
                CollisionType == CollisionEventType.ImpulseOrVelocity && (forceMet || velocityMet))
            {
                ThresholdMet.Invoke();
                if (ThresholdMet.GetPersistentEventCount() < 1)
                {
                    GetComponent<HVRDestructible>().Destroy();

                }
            }
        }
    }

    [Serializable]
    public enum CollisionEventType
    {
        Impulse, Velocity, ImpulseOrVelocity
    }

}
