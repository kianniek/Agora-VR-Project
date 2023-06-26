using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SnapToPlayerHead;

[RequireComponent(typeof(Collider))]
public class SnapToPlayerHead : MonoBehaviour
{
    [SerializeField] string tagToCompare = "MainCamera";
    [SerializeField] bool transitionOnSnap = true;
    [SerializeField] TransportToWorld transportToWorld;
    [SerializeField] Material TransporterMaterial;
    [SerializeField] float maxGrowthScale = 3;

    public enum ZTestValue
    {
        Always,
        Equal,
        Greater,
        GreaterEqual,
        Less,
        LessEqual,
        Never
    }

    public ZTestValue zTestValue = ZTestValue.Always;
    public ZTestValue zTestNormalValue = ZTestValue.LessEqual;

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
            transform.position = Vector3.Lerp(transform.position, collision.transform.position, 0.1f);
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
            SetDepthTest(zTestValue);
            transform.position = Vector3.Lerp(transform.position, other.transform.position, 0.1f);
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0, maxGrowthScale), Mathf.Clamp(transform.localScale.y, 0, maxGrowthScale), Mathf.Clamp(transform.localScale.z, 0, maxGrowthScale));
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag(tagToCompare))
        {
            SetDepthTest(zTestValue);
            transform.position = Vector3.Lerp(transform.position, collision.transform.position, 0.1f);
            transform.localScale += Vector3.one;
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0, maxGrowthScale), Mathf.Clamp(transform.localScale.y, 0, maxGrowthScale), Mathf.Clamp(transform.localScale.z, 0, maxGrowthScale));
        }
    }

    private void OnDestroy()
    {
        SetDepthTest(zTestNormalValue);
    }

    private void OnApplicationQuit()
    {
        SetDepthTest(zTestNormalValue);
    }

    private void OnDisable()
    {
        SetDepthTest(zTestNormalValue);
    }

    void SetDepthTest(ZTestValue SetValue)
    {
        // Get the material of the object
        TransporterMaterial = GetComponent<MeshRenderer>().material;
        Material material = TransporterMaterial;

        // Map the ZTest enum value to the corresponding CompareFunction value
        UnityEngine.Rendering.CompareFunction compareFunction;
        switch (SetValue)
        {
            case ZTestValue.Always:
                compareFunction = UnityEngine.Rendering.CompareFunction.Always;
                break;
            case ZTestValue.Equal:
                compareFunction = UnityEngine.Rendering.CompareFunction.Equal;
                break;
            case ZTestValue.Greater:
                compareFunction = UnityEngine.Rendering.CompareFunction.Greater;
                break;
            case ZTestValue.GreaterEqual:
                compareFunction = UnityEngine.Rendering.CompareFunction.GreaterEqual;
                break;
            case ZTestValue.Less:
                compareFunction = UnityEngine.Rendering.CompareFunction.Less;
                break;
            case ZTestValue.LessEqual:
                compareFunction = UnityEngine.Rendering.CompareFunction.LessEqual;
                break;
            case ZTestValue.Never:
                compareFunction = UnityEngine.Rendering.CompareFunction.Never;
                break;
            default:
                compareFunction = UnityEngine.Rendering.CompareFunction.LessEqual;
                break;
        }
        // Change the ZTest property to the desired value based on the enum value
        material.SetInt("_ZTest", (int)compareFunction);
    }
}
