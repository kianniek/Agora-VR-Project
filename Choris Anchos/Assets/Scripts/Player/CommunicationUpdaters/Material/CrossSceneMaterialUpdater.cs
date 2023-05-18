using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Put this script on any model that has a material on it
/// </summary>
public class CrossSceneMaterialUpdater : MonoBehaviour
{
    [SerializeField] CrossSceneMaterialCopier.Models modelType;
    public Texture2D replacementTexture;
    Renderer meshRenderer;
    [Tooltip("Leave This Blank to use the Default '_BaseMap' property name")]
    [SerializeField] private string materialAlbedoProperty;
    // Start is called before the first frame update
    void Start()
    {
        // Try to get the MeshRenderer component
        meshRenderer = GetComponent<MeshRenderer>();

        // If the MeshRenderer component doesn't exist, try to get the SkinnedMeshRenderer component
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<SkinnedMeshRenderer>();

            // If the SkinnedMeshRenderer component also doesn't exist, disable the script
            if (meshRenderer == null)
            {
                Debug.LogError("MeshRenderer or SkinnedMeshRenderer component not found!");
                enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(modelType == CrossSceneMaterialCopier.modelToChange)
        {
            if (meshRenderer)
            {
                string basemapName = materialAlbedoProperty == "" ? "_BaseMap" : materialAlbedoProperty;
                meshRenderer.material.SetTexture(basemapName, replacementTexture);
            }
        }
    }

    
}

public static class CrossSceneMaterialCopier
{
    //Add models to this enum to make sure the right model material gets updated
    [Serializable]
    public enum Models
    {
        None,
        Left_Hand,
        Right_Hand
    }

    //To use this script you got to read the 'modelToChange' string to check if the right model gets changed
    //The accompenieng script will have to clear this string after it has been read
    public static Models modelToChange;
}
