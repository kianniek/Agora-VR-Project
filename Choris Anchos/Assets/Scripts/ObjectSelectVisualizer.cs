using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectVisualizer : MonoBehaviour
{

    [SerializeField] GameObject[] pillars;
    GameObject[] pillarsPosDupe = new GameObject[4];
    [SerializeField] List<Vector3> pillarsPosStart = new();
    [SerializeField] GameObject closestPillars;
    [SerializeField] Vector3 posOfHands = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pillars.Length; i++)
        {
            pillarsPosStart.Add(new Vector3(pillars[i].transform.position.x, 0, pillars[i].transform.position.z));
        }

        for (int i = 0; i < pillarsPosStart.Count; i++)
        {
            GameObject go3 = new GameObject("go3");
            go3.transform.position = pillarsPosStart[i];
            pillarsPosDupe[i] = go3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        WorldPosAverage();
        closestPillars = GetClosest(pillars);
        float dist = Vector3.Distance(posOfHands, closestPillars.transform.position);
        print(dist);
        if (dist < 1.3f)
        {
            for (int i = 0; i < pillars.Length; i++)
            {
                if (pillars[i] != closestPillars)
                {
                    pillars[i].transform.position -= transform.up / 10;
                }
                else
                {
                    pillars[i].transform.position = pillarsPosStart[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < pillars.Length; i++)
            {
                    pillars[i].transform.position = Vector3.Lerp(pillars[i].transform.position, pillarsPosStart[i], 0.5f);
            }
        }
    }

    GameObject GetClosest(GameObject[] array)
    {
        GameObject tMin = pillars[0];
        float minDist = Mathf.Infinity;
        Vector3 currentPos = posOfHands;
        for (int i = 0; i < pillars.Length; i++)
        {
            float dist = Distcance(pillarsPosDupe[i].transform.position, posOfHands);
            
            if (dist < minDist)
            {
                tMin = pillars[i];
                minDist = dist;
            }
        }
        return tMin;
    }

    float Distcance(Vector3 objectInWorld, Vector3 worldPosOther)
    {
        return Vector3.Distance(objectInWorld, worldPosOther);
    }

    public void WorldPosAverage()
    {
        posOfHands = (HandPosMessager.hand_R + HandPosMessager.hand_L) / 2;
    }
}
