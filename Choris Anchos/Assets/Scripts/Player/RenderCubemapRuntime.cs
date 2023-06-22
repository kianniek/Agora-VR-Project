using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCubemapRuntime : MonoBehaviour
{
    public Transform renderFromPosition;
    public Cubemap cubemap;
    public LayerMask maskout;
    bool used;

    public float timeLimit = 3f;  // The amount of time for the timer
    private float timeLeft;         // The amount of time remaining in the timer
    // Start is called before the first frame update
    private void Start()
    {
        timeLeft = timeLimit;       // Set the time remaining to the limit
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;    // Subtract the time that has passed since the last frame
        if (!used)
        {
           

            if (timeLeft <= 0.0f)          // If the time has run out
            {
                MakeCubemap();
                timeLeft = 0.0f;           // Reset the time remaining to zero
            }
            
        }
    }

    public void MakeCubemap()
    {
        // create temporary camera for rendering
        GameObject go = new GameObject("CubemapCamera");
        go.transform.SetParent(renderFromPosition, true);
        Camera cam = go.AddComponent<Camera>();
        
        cam.cullingMask = maskout;

        // place it on the object
        go.transform.rotation = Quaternion.identity;
        go.transform.position = new Vector3(0, renderFromPosition.position.y, 0);
        // render into cubemap
        go.GetComponent<Camera>().RenderToCubemap(cubemap);

        // destroy temporary camera
        DestroyImmediate(go);
        used = true;
    }
}
