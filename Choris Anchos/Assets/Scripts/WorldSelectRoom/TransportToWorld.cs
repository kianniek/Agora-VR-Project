using HurricaneVR.Framework.Shared;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

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
    [SerializeField] GameObject[] ParentCollider;
    [InspectorButton("GetAllChildrenCollidersFromParent", 250)]
    public bool getAllColliders;
    [SerializeField] Collider[] ColliderChildren;

    [Header("Shader Handler")]
    [SerializeField] public WorldRevealURP worldReveal;
    [SerializeField] public RevealPointManager revealPointManager;
    [Tooltip("time it takes to move")]
    [SerializeField] float mps = 1f; // meter per second expand
    [SerializeField] float maxDiamiter = 10;
    private float timer = 0f; // timer to keep track of time
    private float value;
    // Start is called before the first frame update
    void Start()
    {
        if (worldReveal != null)
        {
            worldReveal.revealRadius = 0.01f;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene()
    {
        //if (worldReveal == null)
        //{
        //    worldReveal = Object.FindObjectOfType<WorldRevealURP>();
        //}
        if (!SceneManager.GetSceneByName(transportToScene).isLoaded)
        {
            StartCoroutine(SceneSwitch());
        }
    }

    IEnumerator SceneSwitch()
    {
        //prevent the worldReveal point to switch in the middle of the transition
        if (revealPointManager) { revealPointManager.closestRevealPedistal.stopEffect = true; }
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(transportToScene, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;

        Scene selfScene = gameObject.scene;
        Scene otherScene = SceneManager.GetSceneByName(transportToScene);
        gameObject.transform.SetParent(null, true);
        SceneManager.MoveGameObjectToScene(gameObject, otherScene);

        yield return new WaitForSeconds(0.3f);

        asyncOperation.allowSceneActivation = true;

        if (worldReveal == null)
        {
            worldReveal = FindObjectOfType<WorldRevealURP>();
        }

        worldReveal.ExpandShaderStart(maxDiamiter, mps);

        if (revealPointManager) { revealPointManager.closestRevealPedistal.stopEffect = false; }

        for (int i = 0; i < ColliderChildren.Length; i++)
        {
            ColliderChildren[i].enabled = false;
        }
        //yield return new WaitWhile(() => worldReveal.revealRadius < 100);

        //print("Name of Old Scene is: " + selfScene.name);

        UnloadAllScenesExcept(transportToScene);

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

    void UnloadAllScenesExcept(string transportToScene)
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
    void GetAllChildrenCollidersFromParent()
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
