using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceObject : MonoBehaviour
{
    public bool addForce = false;
    public Rigidbody body;
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (addForce)
        {
            addForce = false;
            body.AddForce(direction);
        }
    }
}
