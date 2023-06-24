using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderInverter : MonoBehaviour
{

    [SerializeField] bool _defaultValue;

    [SerializeField] bool invert;
    [SerializeField] bool turnback;

    [SerializeField] List<Material> RevealShaderMaterials;

    // Update is called once per frame
#if UNITY_EDITOR
    void Update()
    {
        if (invert)
        {
            Switch();
            invert = false;
        }

        if (turnback)
        {
            SwitchBack();
        }
    }

    void Switch()
    {
        for (int i = 0; i < RevealShaderMaterials.Count; i++)
        {
            Debug.Log("|" +RevealShaderMaterials[i].shader.name + "|");
            if (RevealShaderMaterials[i] == null) 
            { 
                continue; 
            }

            if (RevealShaderMaterials[i].shader.name != "Shader Graphs/RevealShader" && RevealShaderMaterials[i].shader.name != "Shader Graphs/TerrainMusicWorldShader" && RevealShaderMaterials[i].shader.name != "Shader Graphs/RevealShaderGlass")
            {
                RevealShaderMaterials.RemoveAt(i);
                continue;
            }
            float currentBool = 100;
            currentBool = RevealShaderMaterials[i].GetFloat("_Invert");
            if (currentBool == 100) { continue; }
            currentBool = currentBool == 0 ? 1 : 0;
            RevealShaderMaterials[i].SetFloat("_Invert", currentBool);

            //transform.position = new Vector3(transform.position.x,Mathf.Sin(transform.position.y) , transform.position.z);
        }
    }
    void SwitchBack()
    {
        Shader litShader = Shader.Find("Universal Render Pipeline/Lit");

        if (litShader == null)
        {
            Debug.LogError("Shader not found. Make sure it's included in your build.");
            return;
        }

        for (int i = 0; i < RevealShaderMaterials.Count; i++)
        {
            RevealShaderMaterials[i].shader = litShader;
        }
    }

#endif
}
