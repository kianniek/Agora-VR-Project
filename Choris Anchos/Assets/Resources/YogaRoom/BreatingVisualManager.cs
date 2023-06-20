using ExternalPropertyAttributes;
using HurricaneVR.Framework.Shared;
using System;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class BreatingVisualManager : MonoBehaviour
{
    [Serializable]
    public struct LightMod
    {
        public Light breathingLight;
        public float lightMultiplier;
    }

    public bool doBreating;
    [SerializeField][MinMaxSlider(-1, 1)] Vector2 intensitySliderRange;
    [Range(-1f, 1f)]
    public float intensitySlider = 0f;  // Slider to control the intensity of the visualization

    public Transform[] breathingRings; // Array of breathing ring objects
    public float[] ringMultipliers;    // Array of multipliers for the breathing rings
    private readonly Vector3[] ringSavedScale;   // Array of multipliers for the lights


    public LightMod[] breathingLights;    // Array of light objects for breathing visualization
    [InspectorButton("Start")]
    public bool SavedVaribles;
    public List<float> lightSavedIntencity;   // Array of multipliers for the lights

    private void Start()
    {
        lightSavedIntencity = new List<float>();
        // Set the default intensity value
        intensitySlider = 0.5f;

        // Update the breathing rings
        for (int i = 0; i < breathingRings.Length; i++)
        {
            ringSavedScale[i] = breathingRings[i].localScale;
        }

        // Update the breathing lights
        for (int i = 0; i < breathingLights.Length; i++)
        {
            lightSavedIntencity.Add(breathingLights[i].breathingLight.intensity);
        }
    }

    private void Update()
    {
        if (doBreating)
        {
            // Calculate the current intensity based on the slider value and multipliers
            intensitySlider = CalculateBreathingValue();
            // Update the breathing rings
            for (int i = 0; i < breathingRings.Length; i++)
            {
                if (breathingRings.Length > 0)
                {
                    Vector3 currentIntensity = ringSavedScale[i] + Vector3.one * intensitySlider;
                    breathingRings[i].localScale = currentIntensity * ringMultipliers[i];
                }
            }

            // Update the breathing lights
            for (int i = 0; i < breathingLights.Length; i++)
            {
                float currentIntensity = lightSavedIntencity[i];
                breathingLights[i].breathingLight.intensity = currentIntensity + intensitySlider * breathingLights[i].lightMultiplier;
            }
            return;
        }
    }

    public float breathingSpeed = 1f; // Speed of the breathing exercise

    private float timer = 0f;

    private float CalculateBreathingValue()
    {
        timer += Time.deltaTime * breathingSpeed;

        // Calculate the value ranging from -1 to 1 using a sine wave
        float evenwichtstand = (intensitySliderRange.x + intensitySliderRange.y) / 2;
        float amplitude = Mathf.Abs(evenwichtstand - intensitySliderRange.y);
        float breathingValue = Mathf.Sin(timer) * amplitude + evenwichtstand;

        // Clamp the value between -1 and 1
        breathingValue = Mathf.Clamp(breathingValue, intensitySliderRange.x, intensitySliderRange.y);

        // Calculate the value ranging from -1 to 1 using an easing function
        float breathingValueEased = EasingFunctions.EaseInCubic(breathingValue);

        return breathingValue;
    }

    public void BreathingEnabled(bool enabled) { doBreating = enabled; }

}

public static class EasingFunctions
{
    // Easing function: Linear interpolation
    public static float Linear(float t)
    {
        return t;
    }

    // Easing function: Ease In Quad
    public static float EaseInQuad(float t)
    {
        return t * t;
    }

    // Easing function: Ease Out Quad
    public static float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }

    // Easing function: Ease In Out Quad
    public static float EaseInOutQuad(float t)
    {
        return (t < 0.5f) ? (2f * t * t) : (-1f + (4f - 2f * t) * t);
    }

    // Easing function: Ease In Cubic
    public static float EaseInCubic(float t)
    {
        return t * t * t;
    }

    // Easing function: Ease Out Cubic
    public static float EaseOutCubic(float t)
    {
        float f = t - 1f;
        return f * f * f + 1f;
    }

    // Easing function: Ease In Out Cubic
    public static float EaseInOutCubic(float t)
    {
        return (t < 0.5f) ? (4f * t * t * t) : ((t - 1f) * (2f * t - 2f) * (2f * t - 2f) + 1f);
    }

    // Easing function: Ease In Quart
    public static float EaseInQuart(float t)
    {
        return t * t * t * t;
    }

    // Easing function: Ease Out Quart
    public static float EaseOutQuart(float t)
    {
        float f = t - 1f;
        return f * f * f * (1f - t) + 1f;
    }

    // Easing function: Ease In Out Quart
    public static float EaseInOutQuart(float t)
    {
        if (t < 0.5f)
        {
            return 8f * t * t * t * t;
        }
        else
        {
            float f = t - 1f;
            return -8f * f * f * f * f + 1f;
        }
    }
}

