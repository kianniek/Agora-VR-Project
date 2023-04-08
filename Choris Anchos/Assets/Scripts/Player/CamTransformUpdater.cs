using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTransformUpdater : MonoBehaviour
{
    private void FixedUpdate()
    {
        CameraTransformCopier.position = transform.position;
        CameraTransformCopier.rotation = transform.rotation;
        CameraTransformCopier.scale = transform.lossyScale;
    }
}
