using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScanInstantiator : MonoBehaviour
{
    // Serialized fields can be edited in the Unity inspector
    [SerializeField] private GameObject scannerPref; // Prefab for scanner sphere
    [SerializeField] private bool bong = false; // Unused variable
    [SerializeField] private float scannerLifeTime; // Lifetime of scanner sphere
    [SerializeField] float pitchMultiplier; // Multiplier for scanner sphere speed

    Renderer renderer;
    Material material;

    private bool isCoolingDown = false; // Flag for preventing scanner sphere spam
    private void PlayPulse(float pitch)
    {
        if (isCoolingDown)
        {
            return; // don't instantiate a new object if still cooling down
        }

        // Instantiate new scanner sphere and set its speed
        GameObject scannerPulse = Instantiate(scannerPref, this.transform.position, this.transform.rotation);
        ScannerSphere myScanner = scannerPulse.GetComponent<ScannerSphere>();
        myScanner.SetSpeed(Mathf.Pow(pitch, pitchMultiplier));

        // Set up variables for fading out the scanner sphere
        renderer = scannerPulse.GetComponent<Renderer>();
        material = renderer.material;

        // Start coroutine to fade out the scanner sphere
        StartCoroutine(FadeOut(scannerPulse, scannerLifeTime));

        // Start cooldown timer
        isCoolingDown = true;
        StartCoroutine(CoolDown());
    }

    // Coroutine to prevent scanner sphere spam
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.8f);
        isCoolingDown = false; // allow new objects to be instantiated again
    }

    // Coroutine to fade out the scanner sphere over time
    IEnumerator FadeOut(GameObject obj, float duration)
    {
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float progress = (Time.time - startTime) / duration;
            float a = Mathf.Lerp(1f, 0f, progress); // interpolate alpha value from 1 to 0
            material.SetFloat("_Alpha", a);
            yield return null;
        }

        Destroy(obj); // destroy object after fading is complete
    }

    // Public method for setting the pitch of the scanner sphere and playing it
    public void SetPitchAndPlay(float pitch)
    {
        PlayPulse(pitch);
    }
}
