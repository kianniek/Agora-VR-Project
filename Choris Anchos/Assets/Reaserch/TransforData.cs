using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transform Data", menuName = "Transform Data")]
public class TransforData : ScriptableObject
{
    public Vector3List positions = new Vector3List();
    public Vector3List rotations = new Vector3List();
}

[System.Serializable]
public class Vector3List
{
    public List<Vector3> items = new List<Vector3>();
}

[System.Serializable]
public class QuaternionList
{
    public List<Vector3> items = new List<Vector3>();
}