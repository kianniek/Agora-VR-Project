using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabJitterFix : MonoBehaviour
{
    [SerializeField] bool force = false;

    Transform _parent;
    Vector3 _localPosition;

    private void Start()
    {
        _parent = transform.parent;
        _localPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (force || transform.parent != _parent)
            transform.position = _parent.position + _localPosition;
    }
}
