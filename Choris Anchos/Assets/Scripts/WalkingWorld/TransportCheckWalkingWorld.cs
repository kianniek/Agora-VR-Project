using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransportCheckWalkingWorld : MonoBehaviour
{
    [SerializeField] GameObject[] ObjectsToCheckIfDisabled;
    [SerializeField] float delay = 1f; // Delay in seconds before invoking the event

    public UnityEvent onObjectsDisabled; // Unity event to invoke when objects are disabled

    private bool hasInvokedEvent = false; // Flag to track if the event has been invoked

    // Update is called once per frame
    void Update()
    {
        // Check if the event has already been invoked
        if (hasInvokedEvent)
            return;

        // Iterate over the objects and check if any of them are active
        foreach (GameObject obj in ObjectsToCheckIfDisabled)
        {
            if (obj.activeSelf)
                return;
        }
        Debug.Log("StartedTransition");
        // All objects are disabled, invoke the event after the specified delay
        StartCoroutine(InvokeEventWithDelay());
        hasInvokedEvent = true;
    }

    IEnumerator InvokeEventWithDelay()
    {
        yield return new WaitForSeconds(delay);
        onObjectsDisabled.Invoke();
    }
}
