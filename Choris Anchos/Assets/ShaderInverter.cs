using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ShaderInverter : MonoBehaviour
{
    [SerializeField] bool _defaultValue;

    [SerializeField] bool invert;

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

            if (RevealShaderMaterials[i].shader.name != "Shader Graphs/RevealShader")
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
#endif
}
