using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VrButton : MonoBehaviour
{
    public float offsetDown, offsetUp;
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    [SerializeField]GameObject presser;
    [SerializeField]bool isPressed;

    void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, offsetDown, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, offsetUp, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }
}
