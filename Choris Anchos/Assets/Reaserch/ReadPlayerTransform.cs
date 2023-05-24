using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Player;
using UnityEngine.Events;

public class ReadPlayerTransform : MonoBehaviour
{
    public UnityEvent triggerActive;
    public bool analize = true;

    bool temp = false;

    private void Update()
    {
        this.transform.position = PlayerTransform.position;
        this.transform.rotation = Quaternion.Euler(PlayerTransform.rotation);
        if (PlayerTransform.Activate != temp && analize)
        {
            triggerActive.Invoke();
            analize = false;
        }
        else if(PlayerTransform.Activate == temp)
        {
            analize = true;
        }

    }
}

