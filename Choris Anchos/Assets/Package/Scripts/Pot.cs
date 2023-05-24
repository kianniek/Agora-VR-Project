using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public GameObject fracture;
    public GameObject pot;
    public Rigidbody myRb;
    public BoxCollider myCollider;

    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball") || collision.gameObject.CompareTag("fracture"))
        {
            myRb.isKinematic = true;
            myRb.detectCollisions = false;
            myCollider.enabled = false;
            fracture.SetActive(true);
            for (int i = 0; i < fracture.transform.childCount; i++)
            {
                fracture.transform.GetChild(i).GetComponent<Rigidbody>().velocity = collision.gameObject.transform.GetComponent<Rigidbody>().velocity * 5;
            }
            pot.SetActive(false);
        }
    }
}
