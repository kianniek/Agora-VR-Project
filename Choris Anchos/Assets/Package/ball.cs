using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ball : MonoBehaviour
{
    public Rigidbody myRb;
    public Vector3 speed;
    public bool shoot;
    public InputAction myAction;
    // Update is called once per frame
    private void OnEnable()
    {
        myAction.Enable();
    }

    private void OnDisable()
    {
        myAction.Disable();
    }
    void Update()
    {
        if (myAction.ReadValue<float>() > 0.1f && myRb.velocity.magnitude <= 1 && shoot == false)
        {
            shoot = true;
        }
        if(shoot)
        {
            speed *= Random.Range(0.8f, 1.3f);
            shoot = false;
            myRb.AddRelativeForce(speed, ForceMode.VelocityChange);
        }
    }
}
