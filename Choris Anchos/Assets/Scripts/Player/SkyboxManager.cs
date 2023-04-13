using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SkyboxManager : MonoBehaviour
{
    public Material SkyboxMaterial;
    [System.Serializable]
    public struct SkyboxColors
    {
        [Range(0, 5)]
        public float AtmosphereThickness;
        public string sceneName;
        public Color skyColor;
        public Color groundColor;
    }
    public SkyboxColors[] skyboxColors = {
        new SkyboxColors {
            AtmosphereThickness = 1,
            sceneName = "SceneMusic",
            skyColor = new Color(1,0,0.646297f),
            groundColor = new Color(0,0.8231192f,1)
        },
        new SkyboxColors {
            AtmosphereThickness = 0,
            sceneName = "WorldSelectRoom",
            skyColor = new Color(0,0,0),
            groundColor = new Color(0,0,0)
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
                print(scene.name + "|||" + colorIndex);
                break;
            }
        }

        // If we found a matching skybox color, apply it to the skybox
        if (colorIndex >= 0)
        {
            //Material skyboxMaterial = RenderSettings.skybox;
            SkyboxMaterial.SetColor("_SkyTint", skyboxColors[colorIndex].skyColor);
            SkyboxMaterial.SetColor("_GroundColor", skyboxColors[colorIndex].groundColor);
            SkyboxMaterial.SetFloat("_AtmosphereThickness", skyboxColors[colorIndex].AtmosphereThickness);
            print(skyboxColors[colorIndex].skyColor);
            //RenderSettings.skybox = skyboxMaterial;
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
            //Material skyboxMaterial = RenderSettings.skybox;
            SkyboxMaterial.SetColor("_SkyTint", skyboxColors[colorIndex].skyColor);
            SkyboxMaterial.SetColor("_GroundColor", skyboxColors[colorIndex].groundColor);
            SkyboxMaterial.SetFloat("_AtmosphereThickness", skyboxColors[colorIndex].AtmosphereThickness);
            //RenderSettings.skybox = skyboxMaterial;
        }
    }

}
