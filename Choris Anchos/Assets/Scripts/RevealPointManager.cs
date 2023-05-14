using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealPointManager : MonoBehaviour
{
    [SerializeField]
    private const string WreckroomSceneName = "SceneWreckRoom";
    [SerializeField] private Material[] materialsWreckroom;
    [SerializeField]
    private const string MusicRoomSceneName = "SceneMusic";
    [SerializeField] private Material[] materialsMusicRoom;
    [SerializeField]
    private const string WalkingWorldSceneName = "SceneWalking";
    [SerializeField] private Material[] materialsWalkingWorld;
    [SerializeField]
    private const string YogaSceneName = "SceneYoga";
    [SerializeField] private Material[] materialsYoga;



    [SerializeField] WorldRevealURP RevealPoint;
    public ObjectSelectVisualizer ClosestRevealPedistal;

    // Update is called once per frame
    void FixedUpdate()
    {
        RevealPoint.gameObject.transform.SetParent(GetChildByDepth(ClosestRevealPedistal.closestPillars.transform, 2), false);
        RevealPoint.gameObject.transform.localPosition = Vector3.zero;

        TransportToWorld transportToWorld = GetChildByDepth(ClosestRevealPedistal.closestPillars.transform, 1).GetComponent<TransportToWorld>();

        transportToWorld.worldReveal = RevealPoint;

        switch (transportToWorld.transportToScene)
        {
            case WreckroomSceneName:
                RevealPoint.materials.Clear();
                for (int i = 0; i < materialsWreckroom.Length; i++)
                {
                    RevealPoint.materials.Add(materialsWreckroom[i]);
                }
                break;
            case MusicRoomSceneName:
                RevealPoint.materials.Clear();
                for (int i = 0; i < materialsMusicRoom.Length; i++)
                {
                    RevealPoint.materials.Add(materialsMusicRoom[i]);
                }
                break;
            case WalkingWorldSceneName:
                RevealPoint.materials.Clear();
                for (int i = 0; i < materialsWalkingWorld.Length; i++)
                {
                    RevealPoint.materials.Add(materialsWalkingWorld[i]);
                }
                break;
            case YogaSceneName:
                RevealPoint.materials.Clear();
                for (int i = 0; i < materialsYoga.Length; i++)
                {
                    RevealPoint.materials.Add(materialsYoga[i]);
                }
                break;
            default:
                break;
        }
    }

    public Transform GetChildByDepth(Transform parent, int childDown)
    {
        Transform child = parent;
        for (int i = 0; i < childDown && child != null; i++)
        {
            child = child.GetChild(0);

            if (!child.gameObject.activeSelf)
            {

            }
        }
        return child;
    }

    void AttachToClosest()
    {

    }
}
