using HurricaneVR.Framework.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransportToWorld : MonoBehaviour
{
    [Tooltip("Name of scene. (The scene must be in the list of scenes inside of the build settings)")]
    [SerializeField] public string transportToScene;

    [InspectorButton("LoadScene", 250)]
    public bool loadSceneWithoutInteraction;

    public bool destroyObjOnLoaded = false;
    public bool destroyScriptOnLoaded = false;
    public MonoBehaviour[] scriptsTodiable;

    [Header("Collider Handler")]
    [Tooltip("Input: Parent Collider | Output: Parent And Children Collider")]
    [SerializeField] private GameObject[] ParentCollider;
    [InspectorButton("GetAllChildrenCollidersFromParent", 250)]
    public bool getAllColliders;
    [SerializeField] private Collider[] ColliderChildren;
    [SerializeField] bool instantTransition = false;
    [Header("Shader Handler")]
    [SerializeField] public RevealPointManager revealPointManager;
    [Tooltip("time it takes to move")]
    [SerializeField] private float speed = 1f; // meter per second expand
    [SerializeField] private float maxDiamiter = 1000;
    private void Start()
    {
        if (RevealPointManager.Instance == null)
        {
            Debug.LogWarning("RevealPointManager instance is missing or not initialized. Ensure that the RevealPointManager script is attached to a game object in the scene.");
        }
        if (SkyboxManager.Instance == null)
        {
            Debug.LogWarning("SkyboxManager instance is missing or not initialized. Ensure that the SkyboxManager script is attached to a game object in the scene.");
        }
    }
    public void LoadScene()
    {
        if (!SceneManager.GetSceneByName(transportToScene).isLoaded)
        {
            StartCoroutine(SceneSwitch());
        }
    }

    private IEnumerator SceneSwitch()
    {
        if (instantTransition)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(transportToScene, LoadSceneMode.Additive);
            //Don't let the Scene activate until you allow it to

            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            asyncOperation.allowSceneActivation = false;

            Scene selfScene = gameObject.scene;
            Scene otherScene = SceneManager.GetSceneByName(transportToScene);

            //yield return new WaitForSeconds(0.3f);

            asyncOperation.allowSceneActivation = true;

            for (int i = 0; i < ColliderChildren.Length; i++)
            {
                if (ColliderChildren[i] != null)
                {
                    ColliderChildren[i].enabled = false;
                }
            }

            UnloadAllScenesExcept(transportToScene);
        }
        else
        {
            //prevent the worldReveal point to switch in the middle of the transition
            if (RevealPointManager.Instance) { RevealPointManager.Instance.closestRevealPedistal.stopEffect = true; }
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(transportToScene, LoadSceneMode.Additive);
            //Don't let the Scene activate until you allow it to

            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            asyncOperation.allowSceneActivation = false;

            Scene selfScene = gameObject.scene;
            Scene otherScene = SceneManager.GetSceneByName(transportToScene);

            //yield return new WaitForSeconds(0.3f);

            asyncOperation.allowSceneActivation = true;
            SkyboxManager.Instance.UpdateSkyboxColors(otherScene);
            RevealPointManager.Instance.StopAllCoroutines();
            RevealPointManager.Instance.ExpandShaderStart(maxDiamiter, speed);

            if (RevealPointManager.Instance) { RevealPointManager.Instance.closestRevealPedistal.stopEffect = false; }

            for (int i = 0; i < ColliderChildren.Length; i++)
            {
                if (ColliderChildren[i] != null)
                {
                    ColliderChildren[i].enabled = false;
                }
            }

            if (maxDiamiter < 10)
            {
                while (RevealPointManager.Instance.GetRevealRadius() < maxDiamiter * 0.9f)
                {
                    Debug.Log(maxDiamiter * 0.9f);
                    yield return null;
                }
            }
            else
            {
                while (RevealPointManager.Instance.GetRevealRadius() > maxDiamiter + 0.9f)
                {
                    Debug.Log(maxDiamiter * 0.9f);
                    yield return null;
                }
            }


            gameObject.transform.SetParent(null, true);
            SceneManager.MoveGameObjectToScene(gameObject, otherScene);

            RevealPointManager.Instance.GetRevealPoint().transform.SetParent(null, true);
            SceneManager.MoveGameObjectToScene(RevealPointManager.Instance.GetRevealPoint(), otherScene);

            RevealPointManager.Instance.GetRevealPoint().transform.SetParent(gameObject.transform, true);

            UnloadAllScenesExcept(transportToScene);

            //print("Name of Old Scene is: " + selfScene.name);
            if (transportToScene == "WorldSelectRoom" || destroyObjOnLoaded)
            {
                ResetPlayerMessager.ResetPlayer = true;
                yield return new WaitForSeconds(0.3f);
                Destroy(this.gameObject);
            }

            if (destroyScriptOnLoaded)
            {
                yield return new WaitForSeconds(0.3f);
                foreach (MonoBehaviour script in scriptsTodiable)
                {
                    Destroy(script);
                }
            }
        }
    }

    private void UnloadAllScenesExcept(string transportToScene)
    {
        int c = SceneManager.sceneCount;
        for (int i = 0; i < c; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != "ScenePlayer" && scene.name != transportToScene)
            {
                SceneManager.UnloadSceneAsync(scene);
                Debug.Log($"Unloaded scene: " + scene.name);
            }
        }
    }
    public void RemoveScripts()
    {
        foreach (MonoBehaviour script in scriptsTodiable)
        {
            Destroy(script);
        }
    }

    /// <summary>
    /// DONT USE IN RUNTIME | EDITOR ONLY
    /// </summary>
    private void GetAllChildrenCollidersFromParent()
    {
        List<Collider> allChildColliders = new List<Collider>();

        foreach (GameObject parentGameObject in ParentCollider)
        {
            Collider[] childColliders = parentGameObject.GetComponentsInChildren<Collider>();
            allChildColliders.AddRange(childColliders);
        }

        ColliderChildren = allChildColliders.ToArray();
    }
}
