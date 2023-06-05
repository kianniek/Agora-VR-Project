using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderInteractor : MonoBehaviour
{
    public float playerSize;
    public List<GrassObject> objectList; 
    public GameObject objects;
    public GameObject objects2;
    public int index;




    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            Shader.SetGlobalVector("_Interactor" + i, objectList[i].gameObject.transform.position + Vector3.up * objectList[i].playerSize);
            Vector3 position = objectList[i].gameObject.transform.position + Vector3.up * objectList[i].playerSize;
            Vector4 ShaderValues = new Vector4(position.x, position.y, position.z, objectList[i].InteractionRange);
            Shader.SetGlobalVector("_Tester" + i, ShaderValues);
        }

    }

}
