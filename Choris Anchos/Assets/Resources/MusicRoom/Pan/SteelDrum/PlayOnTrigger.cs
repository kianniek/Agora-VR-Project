using HurricaneVR.Framework.Core;
using UnityEngine;

public class PlayOnTrigger : PlayOnCollision
{
    [SerializeField] private bool needsToBeInHand;
    [SerializeField] private bool debug;
    private void OnTriggerEnter(Collider other)
    {
        if (needsToBeInHand)
        {
            HVRGrabbable grabbable = other.GetComponentInParent<HVRGrabbable>();
            debug = grabbable.IsBeingHeld;
            // Check if the other object is being held by a hand
            if (grabbable != null && !grabbable.IsBeingHeld)
            {
                // Object is being held by a hand, execute the collision
                return;
            }
        }
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
