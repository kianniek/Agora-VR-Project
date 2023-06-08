using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 3f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Update()
    {
        // Movement
        float forwardMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        float sidewaysMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;

        Vector3 moveDirection = (transform.forward * forwardMovement) + (transform.right * sidewaysMovement);
        transform.Translate(moveDirection);

        // Rotation
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        rotationX -= mouseY;
        rotationY += mouseX;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
