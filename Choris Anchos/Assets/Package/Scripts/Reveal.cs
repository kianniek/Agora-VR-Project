using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Reveal : MonoBehaviour
{
    public bool reveal = false;
    public InputAction myAction;
    public GameObject revealSphere;
    public float speed = 0.1f;
    public float devide = 10f;
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
        if (myAction.ReadValue<float>() > 0.1f && reveal == false)
        {
            reveal = true;
        }
        if (revealSphere.transform.localScale.x <= 1000 && reveal)
        {
            float extraspeed = revealSphere.transform.localScale.x;
            revealSphere.transform.localScale += new Vector3(speed+extraspeed/ devide, speed+extraspeed / devide, speed+ extraspeed / devide);
            revealSphere.transform.localScale += new Vector3(speed, speed, speed);
        }
    }
}
