using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformreader : MonoBehaviour
{
    public void UpdateLocomotion(bool movement)
    {
        PlayerTransform.teleportation = movement;
        PlayerTransform.locomotion = !movement;
    }
}
