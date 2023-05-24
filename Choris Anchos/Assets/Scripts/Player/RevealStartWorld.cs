using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using HurricaneVR.Framework.ControllerInput;

public class RevealStartWorld : HVRPlayerInputs
{
    public GameObject StartWorldSphere; // The prefab of the object you want to spawn
    public GameObject leftHand;
    public GameObject rightHand;
    public HVRPlayerInputs playerInputs;
    public float stretchThreshold = 0.5f;

    private GameObject startSphere;

    private void Start()
    {
        if (playerInputs == null)
        {
            Debug.LogError("Missing HVRPlayerInputs reference in VRObjectSpawner.");
            return;
        }
    }

    private void Update()
    {
        // Check if the A and X buttons are pressed
        bool aButtonPressed = playerInputs.RightController.PrimaryButtonState.Active;
        bool xButtonPressed = playerInputs.LeftController.PrimaryButtonState.Active;

        if (aButtonPressed && !xButtonPressed)
            Debug.Log("Button A on the right controller is pressed.");
        if (xButtonPressed && !aButtonPressed)
            Debug.Log("Button X on the left controller is pressed.");
        if (xButtonPressed && aButtonPressed)
            Debug.Log("Button A on the right controller and Button X on the left controller are pressed.");

        // Spawn object if conditions are met
        if (aButtonPressed && xButtonPressed && IsArmsStretched())
            SpawnObject();
    }

    private void SpawnObject()
    {
        // Check if object is already spawned and if so, destroy it
        startSphere = GameObject.FindGameObjectWithTag("StartSphere");
        if (startSphere != null)
            Destroy(startSphere);

        // Calculate the position of the sphere and spawn sphere on that position
        Vector3 SpawnPos = Vector3.Lerp(leftHand.transform.position, rightHand.transform.position, 0.5f);
        startSphere = Instantiate(StartWorldSphere, SpawnPos, transform.rotation);
    }

    private bool IsArmsStretched()
    {
        // Get positions of camera and hands
        Vector3 leftPosition = leftHand.transform.position;
        Vector3 rightPosition = rightHand.transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        // Define angle in which the arms are considered stretched to the front
        float angleDownThreshold = 150f;
        float angleUpThreshold = 180f;
        float distanceThreshold = 0.6f;

        // Calculate distance from hands to camera
        Vector3 leftToCamera = (cameraPosition - leftPosition);
        Vector3 rightToCamera = (cameraPosition - rightPosition);

        // Calculate distance from hands to camera in Float
        float leftToCameraFloat = Vector3.Distance(cameraPosition, leftPosition);
        float rightToCameraFloat = Vector3.Distance(cameraPosition, rightPosition);

        Debug.Log("The distance from the left hand to the camera is: " + leftToCameraFloat);
        Debug.Log("The distance from the right hand to the camera is: " + rightToCameraFloat);

        // Calculate angles of the controllers related to camera
        float leftAngle = Vector3.Angle(leftToCamera, Camera.main.transform.forward);
        float rightAngle = Vector3.Angle(rightToCamera, Camera.main.transform.forward);
        Debug.DrawRay(Camera.main.transform.position, leftToCamera, Color.cyan, 10);

        Debug.Log("The angle from the left hand to the camera is: " + leftAngle);
        Debug.Log("The angle from the right hand to the camera is: " + rightAngle);

        // Calculate if arms are stretched to the front and return result
        return leftAngle <= angleUpThreshold && leftAngle >= angleDownThreshold && rightAngle <= angleUpThreshold && rightAngle >= angleDownThreshold && leftToCameraFloat >= distanceThreshold && rightToCameraFloat >= distanceThreshold;
    }
}

