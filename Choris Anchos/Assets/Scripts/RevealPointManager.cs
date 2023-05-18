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



    [SerializeField] WorldRevealURP revealPoint;
    [SerializeField] GameObject revealPointPrefab;
    public ObjectSelectVisualizer closestRevealPedistal;
    private void OnEnable()
    {
        if (!revealPoint) { Instantiate(revealPointPrefab); }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (closestRevealPedistal.stopEffect || !closestRevealPedistal.closestPillars || !revealPoint) { return; }

        revealPoint.gameObject.transform.SetParent(GetChildByDepth(closestRevealPedistal.closestPillars.transform, 2), false);
        revealPoint.gameObject.transform.localPosition = Vector3.zero;

        TransportToWorld transportToWorld = GetChildByDepth(closestRevealPedistal.closestPillars.transform, 1).GetComponent<TransportToWorld>();

        transportToWorld.worldReveal = revealPoint;

        switch (transportToWorld.transportToScene)
        {
            case WreckroomSceneName:
                revealPoint.materials.Clear();
                for (int i = 0; i < materialsWreckroom.Length; i++)
                {
                    revealPoint.materials.Add(materialsWreckroom[i]);
                }
                break;
            case MusicRoomSceneName:
                revealPoint.materials.Clear();
                for (int i = 0; i < materialsMusicRoom.Length; i++)
                {
                    revealPoint.materials.Add(materialsMusicRoom[i]);
                }
                break;
            case WalkingWorldSceneName:
                revealPoint.materials.Clear();
                for (int i = 0; i < materialsWalkingWorld.Length; i++)
                {
                    revealPoint.materials.Add(materialsWalkingWorld[i]);
                }
                break;
            case YogaSceneName:
                revealPoint.materials.Clear();
                for (int i = 0; i < materialsYoga.Length; i++)
                {
                    revealPoint.materials.Add(materialsYoga[i]);
                }
                break;
            default:
                break;
        }
    }

    public Transform GetChildByDepth(Transform parent, int childDown)
    {
        Transform child = parent;
        for (int i = 0; i < childDown && child != null && child.childCount > 0; i++)
        {
            child = child.GetChild(0);
        }
        return child;
    }
}
