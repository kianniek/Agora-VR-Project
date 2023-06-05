using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StoneBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private Collider[] colliders;
    private Rigidbody[] childRigidbodies;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        childRigidbodies = GetComponentsInChildren<Rigidbody>();

        // Adjust rigidbody settings
        rb.mass = 2f;  // Adjust the mass value to your desired weight (increase it for heavier stones)
        rb.drag = 4f;  // Adjust the drag value to control linear movement damping (increase it for more stability)
        rb.angularDrag = 4f;  // Adjust the angular drag value to control rotational movement damping (increase it for more stability)
        rb.useGravity = true;  // Enable gravity for more natural falling and interactions
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;  // Enable continuous collision detection for accurate interactions

        // Disable constraints initially to allow for natural physics interactions
        rb.constraints = RigidbodyConstraints.None;

        // Disable child rigidbodies to prevent wobbling of piled stones
        foreach (Rigidbody childRb in childRigidbodies)
        {
            childRb.isKinematic = true;
        }
    }

    // Method to enable/disable collisions between stones
    public void SetCollisionsEnabled(bool enabled)
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = enabled;
        }
    }

    // Method to enable/disable child rigidbodies of piled stones
    public void SetChildRigidbodiesEnabled(bool enabled)
    {
        foreach (Rigidbody childRb in childRigidbodies)
        {
            childRb.isKinematic = !enabled;
        }
    }
}