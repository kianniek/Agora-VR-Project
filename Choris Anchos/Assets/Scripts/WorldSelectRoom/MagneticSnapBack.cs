using System.Collections;
using UnityEngine;

public class MagneticSnapBack : MonoBehaviour
{
    private Vector3 originalWorldPosition;

    public float snapSpeed = 5f;
    public float snapDistance = 1f;
    private float distance;
    private bool isSnappingBack = false;
    private bool isActive = false;
    private void Update()
    {
        if (!enabled || !isActive)
        {
            return; // Skip the update if the script is disabled
        }

        distance = Vector3.Distance(transform.position, originalWorldPosition);

        if (distance > snapDistance)
        {
            if (distance > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, originalWorldPosition, snapSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = originalWorldPosition;
            }
        }
    }
    public void SetOriginalPosition()
    {
        if (isActive)
        {
            return;
        }
        originalWorldPosition = transform.position;
        isActive = true;
    }
    public void DisableAndRemove()
    {
        Destroy(this); // Remove the MagneticSnapBack script
    }
}
