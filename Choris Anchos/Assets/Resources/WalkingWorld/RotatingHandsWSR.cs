using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingHandsWSR : MonoBehaviour
{
    public float checkDistance = 3;
    public float rotationSpeed = 5f;

    private Quaternion initialRotation;
    Vector3 directionToPlayer;
    private bool isPlayerClose = false;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        // Calculate the direction vector from the objects to the player
        directionToPlayer = CameraTransformCopier.position - transform.position;

        isPlayerClose = directionToPlayer.magnitude < checkDistance;
        
        if (isPlayerClose)
        {
            RotateToObject();
        }
        else
        {
            RotateObjects();
        }
    }

    private void RotateToObject()
    {
        // Calculate the rotation towards the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer.normalized, Vector3.up);
        //Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.normalized.x, 0, directionToPlayer.normalized.z), Vector3.up);

        // Smoothly rotate the object towards the player
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void RotateObjects()
    {
        // Rotate the objects around their own axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;
            transform.rotation = initialRotation; // Reset rotation when the player moves away
        }
    }
}
