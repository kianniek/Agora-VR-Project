using HurricaneVR.Framework.Core.Player;
using HurricaneVR.Framework.Shared;
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

    [Header("Shader Handler")]
    [SerializeField] public WorldRevealURP worldReveal;
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
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(transportToScene, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;

        Scene selfScene = gameObject.scene;
        Scene otherScene = SceneManager.GetSceneByName(transportToScene);
        this.gameObject.transform.SetParent(null, true);
        SceneManager.MoveGameObjectToScene(this.gameObject, otherScene);

        yield return new WaitForSeconds(0.3f);

        asyncOperation.allowSceneActivation = true;

        if (worldReveal == null)
        {
            worldReveal = Object.FindObjectOfType<WorldRevealURP>();
        }

        StartCoroutine(ExpandShader());

        yield return new WaitForSeconds(0.3f);
        
        //print("Name of Old Scene is: " + selfScene.name);
        if (selfScene.name != "ScenePlayer")
        {
            SceneManager.UnloadSceneAsync(selfScene);
        }

        if (transportToScene == "WorldSelectRoom" || destroyObjOnLoaded)
        {
            ResetPlayerMessager.ResetPlayer = true;
            yield return new WaitForSeconds(0.3f);
            Destroy(this.gameObject);
        }
    }

    IEnumerator ExpandShader()
    {

        // initialize timer
        float timer = 0f;

        while (timer < (maxDiamiter / mps))
        {
            // increase timer
            timer += Time.deltaTime;

            // calculate the percentage of time elapsed
            float percentageComplete = timer / (maxDiamiter / mps);
            //animCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
            // move the object based on percentage complete
            worldReveal.revealRadius = Mathf.Lerp(worldReveal.revealRadius, maxDiamiter, percentageComplete);

            // wait for the next frame
            yield return null;
        }
        worldReveal.revealRadius = maxDiamiter;
        ///yield return true;
    }

    void OnGUI()
    {
        //Whereas pressing this Button loads the Additive Scene.
        if (GUI.Button(new Rect(20, 60, 150, 30), "Other Scene Additive"))
        {
            LoadScene();
        }
    }

    //public bool loadSceneWithoutInteraction = false;
    //void OnValidate()
    //{
    //    if (pseudoButton)
    //    {
    //        ExampleMethod();

    //        pseudoButton = false;
    //    }
    //}
}
