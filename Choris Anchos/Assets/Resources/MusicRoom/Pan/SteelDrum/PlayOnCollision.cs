using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayOnCollision : MonoBehaviour
{
    [Serializable]
    public struct DelayedUnityEvent
    {
        public float Delay;
        public UnityEvent eventsDelay;
    }
    [SerializeField] UnityEvent events;
    [SerializeField] DelayedUnityEvent[] eventsDelayed;
    public float cooldownTime = 30.0f;
    public bool canInvoke = true;
    public LayerMask collisionLayer;
    [Header("Which hand can be collided with")]
    public bool leftHand = true;
    public bool rightHand = true;
    public bool allowNoHandCollision = true;

    private void OnCollisionEnter(Collision collision)
    {
        bool canExecuteCollision = false;
        HandPosisionInWorld.Hand handCollided;
        var handPosition = collision.collider.GetComponentInParent<HandPosisionInWorld>();

        if (handPosition != null)
        {
            handCollided = handPosition.GetHandType();

            if (handCollided == HandPosisionInWorld.Hand.Left && leftHand)
            {
                canExecuteCollision = true;
            }
            else if (handCollided == HandPosisionInWorld.Hand.Right && rightHand)
            {
                canExecuteCollision = true;
            }
        }
        else if (allowNoHandCollision)
        {
            canExecuteCollision = true;
        }
        // Check if the collision object's layer is included in the specified layer mask
        if (canInvoke && collisionLayer.ContainsLayer(collision.gameObject.layer) && canExecuteCollision)
        {
            canInvoke = false;
            StartCoroutine(InvokeEventWithCooldown());
        }
    }

    IEnumerator InvokeEventWithCooldown()
    {
        events.Invoke();
        for (int i = 0; i < eventsDelayed.Length; i++)
        {
            StartCoroutine(InvokeDelayedEvent(eventsDelayed[i]));
        }
        StartCoroutine(ResetInvokeCooldown());
        yield return null;
    }

    IEnumerator ResetInvokeCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        canInvoke = true;
    }

    IEnumerator InvokeDelayedEvent(DelayedUnityEvent delayedUnityEvent)
    {
        yield return new WaitForSeconds(delayedUnityEvent.Delay);
        delayedUnityEvent.eventsDelay.Invoke();
        StartCoroutine(ResetInvokeCooldown());
        yield return null;
    }
}

public static class LayerMaskExtensions
{
    public static bool ContainsLayer(this LayerMask layerMask, int layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
