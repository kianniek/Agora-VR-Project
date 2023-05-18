using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneMaterialSetter : MonoBehaviour
{
    [SerializeField] CrossSceneMaterialCopier.Models models;
    // Start is called before the first frame update
    public void SetModelToChange()
    {
        CrossSceneMaterialCopier.modelToChange = models;
    }
}
