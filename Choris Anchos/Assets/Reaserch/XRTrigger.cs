using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class XRTrigger : MonoBehaviour
{
    public InputDeviceCharacteristics deviceCharacteristics;
    public InputFeatureUsage<float> triggerUsage;
    public UnityEvent TriggerEvent;

    void Update()
    {
        // Get the list of devices that match the specified characteristics
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, devices);

        // Check if there are any devices found
        if (devices.Count > 0)
        {
            // Get the first device in the list
            InputDevice device = devices[0];

            // Check if the device supports the trigger feature
            if (device.TryGetFeatureValue(triggerUsage, out float triggerValue))
            {
                // Check if the trigger value is greater than 0.5
                if (triggerValue > 0.5f)
                {
                    TriggerEvent.Invoke();
                }
            }
        }
    }
}
