using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyboxManager : MonoBehaviour
{
    [System.Serializable]
    public struct SkyboxTypes
    {
        public string _Scene;
        public Color _Sky;
        public Color _Ground;
    }
    public SkyboxTypes[] skyboxTypes;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < skyboxTypes.Length; i++)
        {
            if(SceneManager.GetSceneByName(skyboxTypes[i]._Scene).buildIndex == -1)
            {
                Debug.LogWarning((skyboxTypes[i]._Scene + " is not a valid scene! /n Try to add a scene in the build or check if it is spelled the right way?")
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
