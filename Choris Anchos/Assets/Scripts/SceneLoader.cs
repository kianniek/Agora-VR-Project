using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private AsyncOperation async;
    public string SceneToLoadOnStart;

    [SerializeField] private GameObject[] objectsToDiableAfterLoad;

    private void Start()
    {
        print("test");
        if (!SceneManager.GetSceneByName(SceneToLoadOnStart).isLoaded)
        {
            async = SceneManager.LoadSceneAsync(SceneToLoadOnStart, LoadSceneMode.Additive);
            print(string.Format("loading {0}...", SceneToLoadOnStart));
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
            }
        }
    }
}
