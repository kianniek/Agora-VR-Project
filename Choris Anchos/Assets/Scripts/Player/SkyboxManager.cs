using UnityEngine;
using UnityEngine.SceneManagement;
[ExecuteInEditMode]
public class SkyboxManager : MonoBehaviour
{
    // Singleton instance
    public static SkyboxManager Instance { get; private set; }
    public Material SkyboxMaterial;

    [System.Serializable]
    public struct SkyboxColors
    {
        [Tooltip("If used only this will be used")]
        public Material skyMaterial;
        [Space(10)]
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
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ignore the PlayerScene
        if (scene.name.Equals("ScenePlayer"))
        {
            return;
        }

        SceneManager.SetActiveScene(scene);
        // Find the index of the skybox color to use based on the scene name
        int colorIndex = -1;
        for (int i = 0; i < skyboxColors.Length; i++)
        {
            if (scene.name.Equals(skyboxColors[i].sceneName))
            {
                colorIndex = i;
                //print(scene.name + "|||" + colorIndex);
                break;
            }
        }

        // If we found a matching skybox color, apply it to the skybox
        if (colorIndex >= 0)
        {
            if (skyboxColors[colorIndex].skyMaterial == null)
            {
                RenderSettings.skybox = SkyboxMaterial;
                //Material skyboxMaterial = RenderSettings.skybox;
                SkyboxMaterial.SetColor("_SkyTint", skyboxColors[colorIndex].skyColor);
                SkyboxMaterial.SetColor("_GroundColor", skyboxColors[colorIndex].groundColor);
                SkyboxMaterial.SetFloat("_AtmosphereThickness", skyboxColors[colorIndex].AtmosphereThickness);
            }
            else
            {
                RenderSettings.skybox = skyboxColors[colorIndex].skyMaterial;
            }
        }
    }

    public void UpdateSkyboxColors(Scene scene)
    {
        // Ignore the PlayerScene
        if (scene.name.Equals("ScenePlayer"))
        {
            return;
        }
        SceneManager.SetActiveScene(scene);
        // Find the index of the skybox color to use based on the scene name
        int colorIndex = -1;
        for (int i = 0; i < skyboxColors.Length; i++)
        {
            if (scene.name.Equals(skyboxColors[i].sceneName))
            {
                colorIndex = i;
                //print(scene.name + "|||" + colorIndex);
                break;
            }
        }

        // If we found a matching skybox color, apply it to the skybox
        if (colorIndex >= 0)
        {
            if (skyboxColors[colorIndex].skyMaterial == null)
            {
                RenderSettings.skybox = SkyboxMaterial;
                //Material skyboxMaterial = RenderSettings.skybox;
                SkyboxMaterial.SetColor("_SkyTint", skyboxColors[colorIndex].skyColor);
                SkyboxMaterial.SetColor("_GroundColor", skyboxColors[colorIndex].groundColor);
                SkyboxMaterial.SetFloat("_AtmosphereThickness", skyboxColors[colorIndex].AtmosphereThickness);
            }
            else
            {
                RenderSettings.skybox = skyboxColors[colorIndex].skyMaterial;
            }
        }
    }

}
