using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FlamePosition : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    public Transform Position;
    public GameObject camera;
    public float damping;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Position.position + offset;
        var lookPos = transform.position - camera.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
}
