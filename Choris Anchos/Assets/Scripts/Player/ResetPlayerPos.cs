using HurricaneVR.Framework.Core.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPos : MonoBehaviour
{
    public HVRPlayerController playerController;
    void Update()
    {
        if (ResetPlayerMessager.ResetPlayer)
        {
            playerController.transform.position = new Vector3(0, playerController.transform.position.y, 0);
            //playerController.transform.rotation = Quaternion.identity;
            CrossSceneMaterialCopier.modelToChange = CrossSceneMaterialCopier.Models.ResetTexture;

            playerController.PreviousPosition = transform.position;
            ResetPlayerMessager.ResetPlayer = !ResetPlayerMessager.ResetPlayer;
        }
    }
}
