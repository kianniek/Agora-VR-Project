using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class WorldRevealURP : MonoBehaviour
{
    [Header("References"), Tooltip("Materials related to the current type of shader")]
    public List<Material> materials = new();
    [Space(20)]
    [SerializeField]
    [Tooltip("Controls the radius of the revealed area")]
    [Min(0.01f)]
    public float revealRadius = 0.01f;

    public bool invert;

    /// <summary>
    /// Call this function for updating the shader properties.
    /// </summary>
    public void UpdateValues()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i] == null) continue;
            materials[i].SetVector("_RevealPosition", transform.position);
            materials[i].SetFloat("_Amount", revealRadius);

            //transform.position = new Vector3(transform.position.x,Mathf.Sin(transform.position.y) , transform.position.z);
        }
    }

    private void Update()
    {
        UpdateValues();
        UpdateTransform();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, revealRadius);
    }

    void UpdateTransform()
    {
        transform.localScale = Vector3.one * revealRadius;
    }
    private void OnApplicationQuit()
    {
        revealRadius = 0;
    }
}

