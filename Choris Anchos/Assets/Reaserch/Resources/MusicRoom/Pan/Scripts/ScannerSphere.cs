using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerSphere : MonoBehaviour
{
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale += speed * Vector3.one;
    }

    public void SetSpeed(float getSpeed)
    {
        speed = getSpeed;
    }
}
