using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(SkyboxManager))]
public class SkyboxManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SkyboxManager skyboxManager = (SkyboxManager)target;
        if (GUILayout.Button("Update Skybox Colors"))
        {
            Scene activeScene = GetActiveSceneExcludingPlayerScene();
            if (activeScene.isLoaded) // check if a valid scene was returned
            {
                skyboxManager.UpdateSkyboxColors(activeScene);
            }
        }
    }

    private Scene GetActiveSceneExcludingPlayerScene()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && scene.name != "ScenePlayer")
            {
                return scene;
            }
        }
        return new Scene(); // return a default (invalid) scene if no suitable scene is found
    }
}
