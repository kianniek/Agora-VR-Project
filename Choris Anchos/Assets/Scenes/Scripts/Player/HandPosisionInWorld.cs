using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosisionInWorld : MonoBehaviour
{
    enum Hand
    {
        Left,
        Right
    }

    [SerializeField] Hand handType = Hand.Right;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        switch (handType)
        {
            case Hand.Left:
                HandPosMessager.hand_L = transform.position;
                break;
            case Hand.Right:
                HandPosMessager.hand_R = transform.position;
                break;
            default:
                break;
        }
    }
}
