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
        this.transform.localScale += new Vector3(speed, speed, speed);
    }

    public void setSpeed(float getSpeed)
    {
        speed = getSpeed;
    }
}
