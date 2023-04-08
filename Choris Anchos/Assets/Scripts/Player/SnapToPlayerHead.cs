using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class SnapToPlayerHead : MonoBehaviour
{
    [SerializeField] string tagToCompare = "MainCamera";
    [SerializeField] bool transitionOnSnap = true;
    [SerializeField] TransportToWorld transportToWorld;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToCompare))
        {
            transform.position = other.transform.position;
            if (transitionOnSnap && transportToWorld)
            {
                transportToWorld.LoadScene();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(tagToCompare))
        {
            transform.position = collision.transform.position;
            if (transitionOnSnap && transportToWorld)
            {
                transportToWorld.LoadScene();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(tagToCompare))
        {
            transform.position = other.transform.position;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag(tagToCompare))
        {
            transform.position = collision.transform.position;
        }
    }
}
