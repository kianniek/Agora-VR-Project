using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SkyboxManager : MonoBehaviour
{
    [System.Serializable]
    public struct SkyboxColors
    {
        public string sceneName;
        public Color skyColor;
        public Color groundColor;
    }
    public SkyboxColors[] skyboxColors = {
        new SkyboxColors {
            sceneName = "SceneMusic",
            skyColor = new Color(0.37f, 0.31f, 0.72f),
            groundColor = new Color(0.14f, 0.19f, 0.36f)
        },
        new SkyboxColors {
            sceneName = "WorldSelectRoom",
            skyColor = new Color(0,0,0),
            groundColor = new Color(1,1,1)
        }
    };

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the index of the skybox color to use based on the scene name
        int colorIndex = -1;
        for (int i = 0; i < skyboxColors.Length; i++)
        {
            if (scene.name.Equals(skyboxColors[i].sceneName))
            {
                colorIndex = i;
                print(scene.name);
                break;
            }
        }

        // If we found a matching skybox color, apply it to the skybox
        if (colorIndex >= 0)
        {
            Material skyboxMaterial = RenderSettings.skybox;
            skyboxMaterial.SetColor("_SkyTint", skyboxColors[colorIndex].skyColor);
            skyboxMaterial.SetColor("_GroundColor", skyboxColors[colorIndex].groundColor);
            RenderSettings.skybox = skyboxMaterial;
        }
    }

    public void UpdateSkyboxColors()
    {
        // Find the index of the skybox color to use based on the scene name
        int colorIndex = -1;
        for (int i = 0; i < skyboxColors.Length; i++)
        {
            if (SceneManager.GetActiveScene().name.Equals(skyboxColors[i].sceneName))
            {
                colorIndex = i;
                break;
            }
        }

        // If we found a matching skybox color, apply it to the skybox
        if (colorIndex >= 0)
        {
            Material skyboxMaterial = RenderSettings.skybox;
            skyboxMaterial.SetColor("_SkyTint", skyboxColors[colorIndex].skyColor);
            skyboxMaterial.SetColor("_GroundColor", skyboxColors[colorIndex].groundColor);
            RenderSettings.skybox = skyboxMaterial;
        }
    }

}
