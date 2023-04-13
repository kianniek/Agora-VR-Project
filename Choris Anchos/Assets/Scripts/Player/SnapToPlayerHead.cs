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
            transform.position = Vector3.Lerp(transform.position, other.transform.position, 0.1f);
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
            transform.position = Vector3.Lerp(transform.position, collision.transform.position ,0.1f);
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
            transform.position = Vector3.Lerp(transform.position, other.transform.position, 0.1f);
            transform.localScale += Vector3.one;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag(tagToCompare))
        {
            transform.position = Vector3.Lerp(transform.position, collision.transform.position, 0.1f);
            transform.localScale += Vector3.one;
        }
    }

    void Transition(Vector3 pos)
    {

    }
}
