using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectVisualizer : MonoBehaviour
{

    [SerializeField] GameObject[] pillars;
    [SerializeField] float maxAmountDown = 3;
    [SerializeField] float minDistance = 1.3f;
    GameObject debugPoint;
    GameObject[] pillarsPosDupe = new GameObject[4];
    List<Vector3> pillarsPosStart;
    GameObject closestPillars;
    Vector3 posOfHands = Vector3.zero;

    [SerializeField] bool debugMode = false;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(this.gameObject.scene);
        pillarsPosStart = new List<Vector3>();
        for (int i = 0; i < pillars.Length; i++)
        {
            pillarsPosStart.Add(new Vector3(pillars[i].transform.position.x, 0, pillars[i].transform.position.z));
        }

        for (int i = 0; i < pillarsPosStart.Count; i++)
        {
            GameObject go = new GameObject("go_" + i);
            go.transform.position = pillarsPosStart[i];
            pillarsPosDupe[i] = go;
        }

        if (debugMode)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.localScale = Vector3.one * 0.1f;
            debugPoint = go;
        }
    }

    // Update is called once per frame
    void Update()
    {
        WorldPosAverage();
        closestPillars = GetClosest(pillars);
        float dist = debugMode ? Vector3.Distance(debugPoint.transform.position, closestPillars.transform.position) : Vector3.Distance(posOfHands, closestPillars.transform.position);
        float moveAmount = (minDistance - dist) * 0.1f; // calculate the amount to move the pillars down
        moveAmount = Mathf.Abs(moveAmount);

        if (dist < minDistance)
        {
            for (int i = 0; i < pillars.Length; i++)
            {
                if (pillars[i] != closestPillars)
                {
                    Vector3 targetPos = pillarsPosStart[i] - maxAmountDown * moveAmount * transform.up;

                    pillars[i].transform.position = Vector3.Lerp(pillars[i].transform.position, targetPos, 0.1f);
                }
                else
                {
                    pillars[i].transform.position = Vector3.Lerp(pillars[i].transform.position, pillarsPosStart[i], 0.1f);
                }
            }
        }
        else
        {
            for (int i = 0; i < pillars.Length; i++)
            {
                pillars[i].transform.position = Vector3.Lerp(pillars[i].transform.position, pillarsPosStart[i], 0.1f);
            }
        }

        if (debugMode)
        {
            //debugPoint.transform.position = posOfHands;
        }
    }

    GameObject GetClosest(GameObject[] array)
    {
        GameObject tMin = pillars[0];
        float minDist = Mathf.Infinity;
        Vector3 currentPos = posOfHands;
        for (int i = 0; i < pillars.Length; i++)
        {
            float dist = !debugMode ? Vector3.Distance(pillarsPosDupe[i].transform.position, posOfHands) : Vector3.Distance(pillarsPosDupe[i].transform.position, debugPoint.transform.position);
            if (dist < minDist)
            {
                tMin = pillars[i];
                minDist = dist;
            }
        }
        return tMin;
    }

    public void WorldPosAverage()
    {
        posOfHands = (HandPosMessager.hand_R + HandPosMessager.hand_L) / 2;
    }
}
