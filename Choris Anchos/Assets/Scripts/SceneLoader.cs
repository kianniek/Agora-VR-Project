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
            print(string.Format("loading {0}...", SceneToLoadOnStart));
        }
        else
        {
            foreach (GameObject item in objectsToDiableAfterLoad)
            {
                item.SetActive(false);
            }
            Destroy(gameObject);
        }
        
    }


    private void Update()
    {
        if (async != null)
        {
            if (async.isDone)
            {
                SkyboxManager.Instance.UpdateSkyboxColors(SceneManager.GetSceneByName(SceneToLoadOnStart));
                foreach (GameObject item in objectsToDiableAfterLoad)
                {
                    item.SetActive(false);
                }
                Destroy(gameObject);
            }
        }
    }
}
