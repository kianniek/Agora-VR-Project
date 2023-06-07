using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

[ExecuteInEditMode]
public class ShaderInteractor : MonoBehaviour
{
    public GameObject player;
    public float playerSize;
    public float playerInteractionRange = 10;
    public List<GrassObject> objectList;
    public float amountOfRenderedObjects = 8;
    public int index;
    private float tempDistance;

    // Update is called once per frame
    void Update()
    {

        // Sort the objectsList based on the distances to the target position
        objectList.Sort((a, b) => Vector3.Distance(a.gameObject.transform.position, player.transform.position).CompareTo(Vector3.Distance(b.gameObject.transform.position, player.transform.position)));
        for (int i = 0; i < amountOfRenderedObjects; i++)
        {

            Shader.SetGlobalVector("_Interactor" + i, objectList[i].gameObject.transform.position + Vector3.up * objectList[i].playerSize);
            Vector3 position = objectList[i].gameObject.transform.position + Vector3.up * objectList[i].playerSize;
            Vector4 ShaderValues = new Vector4(position.x, position.y, position.z, objectList[i].InteractionRange);
            Shader.SetGlobalVector("_Tester" + i, ShaderValues);

        }

    }

}
