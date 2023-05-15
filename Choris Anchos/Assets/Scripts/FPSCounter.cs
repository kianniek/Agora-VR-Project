using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text fpsText;
    public float updateInterval = 0.5f;

    private float frameCount = 0;
    private float elapsedTime = 0f;
    private float currentFps = 0f;

    private void Start()
    {
        if (fpsText == null)
        {
            Debug.LogError("FPSCounter: FPS Text UI is not assigned!");
            enabled = false;
            return;
        }

        fpsText.text = "FPS: ";
    }

    private void Update()
    {
        frameCount++;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= updateInterval)
        {
            currentFps = frameCount / elapsedTime;
            frameCount = 0;
            elapsedTime = 0f;

            fpsText.text = "FPS: " + Mathf.RoundToInt(currentFps).ToString();
        }
    }
}

