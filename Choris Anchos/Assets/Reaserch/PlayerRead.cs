using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Player;

public class PlayerRead : MonoBehaviour
{

    public HVRPlayerInputs playerInput;
    public Vector3 thisPosition, targetPosition;
    private void Update()
    {
        thisPosition = this.transform.position;
        targetPosition = PlayerTransform.position;
        PlayerTransform.position = this.transform.position;
        PlayerTransform.rotation = this.transform.eulerAngles;
        if (playerInput.IsLeftTriggerHoldActive || playerInput.IsRightTriggerHoldActive)
        {
            PlayerTransform.Activate = true;
        }
        else
        {
            PlayerTransform.Activate = false;
        }
    }
}
