using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayOnCollision : MonoBehaviour
{
    [SerializeField] UnityEvent events;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        events.Invoke();
    }
}
