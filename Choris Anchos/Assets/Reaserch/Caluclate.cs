using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caluclate : MonoBehaviour
{
    public TransforData Scriptable;
    public GameObject BaseObject;
    // Start is called before the first frame update
    private struct DistanceAndAngle
    {
        public float distance;
        public float angle;

        public DistanceAndAngle(float distance, float angle)
        {
            this.distance = distance;
            this.angle = angle;
        }
    }

    private void Start()
    {
        // Calculate distances and angles
        Vector3 targetPosition = BaseObject.transform.position; // Replace with your desired target position

        List<DistanceAndAngle> distanceAngleList = new List<DistanceAndAngle>();

        foreach (Vector3 vector in Scriptable.positions.items)
        {
            float distance = Vector3.Distance(vector, targetPosition);
            float angle = Vector3.Angle(vector - targetPosition, transform.forward);
            distanceAngleList.Add(new DistanceAndAngle(distance, angle));
        }

        // Find 10 smallest distances with their respective angles
        Debug.Log("10 Smallest Distances:");
        for (int i = 0; i < 10; i++)
        {
            DistanceAndAngle smallest = FindSmallestDistance(distanceAngleList);
            distanceAngleList.Remove(smallest);
            Debug.Log("Distance: " + smallest.distance + ", Angle: " + smallest.angle);
        }

        // Find 10 largest distances with their respective angles
        Debug.Log("10 Largest Distances:");
        for (int i = 0; i < 10; i++)
        {
            DistanceAndAngle largest = FindLargestDistance(distanceAngleList);
            distanceAngleList.Remove(largest);
            Debug.Log("Distance: " + largest.distance + ", Angle: " + largest.angle);
        }
    }

    private DistanceAndAngle FindSmallestDistance(List<DistanceAndAngle> distanceAngleList)
    {
        DistanceAndAngle smallest = distanceAngleList[0];

        for (int i = 1; i < distanceAngleList.Count; i++)
        {
            if (distanceAngleList[i].distance < smallest.distance)
            {
                smallest = distanceAngleList[i];
            }
        }

        return smallest;
    }

    private DistanceAndAngle FindLargestDistance(List<DistanceAndAngle> distanceAngleList)
    {
        DistanceAndAngle largest = distanceAngleList[0];

        for (int i = 1; i < distanceAngleList.Count; i++)
        {
            if (distanceAngleList[i].distance > largest.distance)
            {
                largest = distanceAngleList[i];
            }
        }

        return largest;
    }

}
