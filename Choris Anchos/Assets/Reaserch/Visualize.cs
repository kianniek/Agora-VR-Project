using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualize : MonoBehaviour
{
    public Transform origin;         // GameObject to cast the rays from
    public List<Vector3> targets;
    public TransforData Scriptable;// List of target positions for the raycasts

    private void OnDrawGizmos()
    {
        if (origin == null)
            return;

        // Set the starting point of the raycast as the position of the origin GameObject
        Vector3 raycastOrigin = origin.position;

        foreach (Vector3 target in Scriptable.positions.items)
        {
            // Perform the raycast
            RaycastHit hit;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(raycastOrigin, target);
        }
    }
}
