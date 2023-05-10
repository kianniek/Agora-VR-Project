using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Place this script on a Gameobject that has multiple Raise Mesh scripts on it to change the height off all children at once
/// </summary>
public class RaiseMeshManagerAsync : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    public float childHeight = 0.5f;
    [SerializeField]
    public Transform playerPos;
    [SerializeField]
    Vector3 posOld = new(0, 0, 0);
    [SerializeField]
    RaiseMeshAsync[] raiseMeshesAsync;
    // Start is called before the first frame update
    void Start()
    {
        if (raiseMeshesAsync.Length == 0)
        {
            raiseMeshesAsync = GetComponentsInChildren<RaiseMeshAsync>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool updateHeight = false;
        if (posOld != playerPos.position)
        {
            updateHeight = true;
            print("posOld != playerPos.position");
        }
        else
        {
            foreach (RaiseMeshAsync raiseMesh in raiseMeshesAsync)
            {
                if (raiseMesh.raiseAmount != childHeight)
                {
                    updateHeight = true;
                    break;
                }
            }
        }



        if (updateHeight)
        {
            //childHeight = raiseMeshes[0].raiseAmount;
            foreach (RaiseMeshAsync raiseMesh in raiseMeshesAsync)
            {
                if(raiseMesh.enabled == false) { continue; }
                raiseMesh.raiseAmount = childHeight;
                raiseMesh.playerPos = playerPos;
                posOld = playerPos.position;
            }
        }

    }

}
