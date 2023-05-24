using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    AsyncOperation async;
    public string SceneToLoadOnStart;

    [SerializeField] GameObject[] objectsToDiableAfterLoad;

    private void Start()
    {
        if (!SceneManager.GetSceneByName(SceneToLoadOnStart).isLoaded)
        {
            async = SceneManager.LoadSceneAsync(SceneToLoadOnStart, LoadSceneMode.Additive);
        }
        
    }

    private void Update()
    {
        if (async != null)
        {
            if (async.isDone)
            {
                foreach (GameObject item in objectsToDiableAfterLoad)
                {
                    item.SetActive(false);
                }
            }
        }
    }
}
