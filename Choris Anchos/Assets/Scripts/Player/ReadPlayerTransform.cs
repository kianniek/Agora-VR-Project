using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Player;

public class ReadPlayerTransform : MonoBehaviour
{
    public HVRPlayerInputs player;
    public HVRTeleporter playerTP;
    
    bool temp = false;

    private void Update()
    {
        if (PlayerTransform.locomotion != temp)
        {
            temp = PlayerTransform.locomotion;
            player.canLocomotion = PlayerTransform.locomotion;
            playerTP.enabled = PlayerTransform.teleportation;
        }
    }
}
