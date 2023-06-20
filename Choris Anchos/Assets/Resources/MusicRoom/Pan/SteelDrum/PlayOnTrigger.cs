using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayOnTrigger : PlayOnCollision
{
    private void OnTriggerEnter(Collider other)
    {
        bool canExecuteCollision = false;
        HandPosisionInWorld.Hand handCollided;
        var handPosition = other.GetComponentInParent<HandPosisionInWorld>();

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
        if (canInvoke && collisionLayer.ContainsLayer(other.gameObject.layer) && canExecuteCollision)
        {
            canInvoke = false;
            StartCoroutine(InvokeEventWithCooldown());
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        
    }
}
