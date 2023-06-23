using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceFollowPlayer : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position = player.transform.position;
    }
}
