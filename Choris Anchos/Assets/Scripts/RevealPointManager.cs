using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealPointManager : MonoBehaviour
{
    private static RevealPointManager instance; // Singleton instance

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

    bool expandWRP = false;
    [SerializeField] WorldRevealURP revealPoint;
    [SerializeField] GameObject revealPointPrefab;
    public ObjectSelectVisualizer closestRevealPedistal;
    TransportToWorld transportToWorld;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public static RevealPointManager Instance
    {
        get { return instance; }
    }
    private void OnEnable()
    {
        if (!revealPoint) { Instantiate(revealPointPrefab); }
    }
    // Update is called once per frame
    void Update()
    {
        if (closestRevealPedistal.stopEffect || !closestRevealPedistal.closestPillars || !revealPoint) { return; }

        revealPoint.gameObject.transform.SetParent(GetChildByDepth(closestRevealPedistal.closestPillars.transform, 1), false);
        revealPoint.gameObject.transform.localPosition = Vector3.zero;

        transportToWorld = closestRevealPedistal.closestPillars.transform.GetComponentInChildren<TransportToWorld>();

        if (transportToWorld != null)
        {
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
    }
    public void ExpandShaderStart(float maxDiamiter, float speed)
    {
        print("ExpandStart");
        StartCoroutine(ExpandShader(maxDiamiter, speed));
    }
    public void ExpandShaderStop()
    {
        print("ExpandStart");
        StopAllCoroutines();
    }
    IEnumerator ExpandShader(float maxDiamiter, float speed)
    {
        print("start setting the revealPoint.revealRadius to " + maxDiamiter);

        // initialize timer
        float timer = 0f;
        while (timer < speed && revealPoint.revealRadius != maxDiamiter)
        {
            // increase timer
            timer += Time.deltaTime;

            // calculate the percentage of time elapsed
            float percentageComplete = timer / speed;


            //animCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
            // move the object based on percentage complete
            revealPoint.revealRadius = Mathf.Lerp(revealPoint.revealRadius, maxDiamiter, percentageComplete);
            
            // wait for the next frame
            yield return null;
        }
        print("Done setting the revealPoint.revealRadius to " + maxDiamiter);
        revealPoint.revealRadius = maxDiamiter;
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

    public float GetRevealRadius()
    {
        return revealPoint.revealRadius;
    }

    public GameObject GetRevealPoint()
    {
        return revealPoint.gameObject;
    }
}
