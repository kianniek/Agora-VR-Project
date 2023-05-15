using System.Linq;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

public class RaiseMesh : MonoBehaviour
{
    public MeshFilter mf;
    public MeshCollider mc;
    private Mesh mesh;

    public ComputeShader raiseShader;
    public ComputeShader initializeShader;

    public float raiseAmount = 1.0f;
    float raiseAmountCheck;
    public Transform playerPos;
    Vector3 playerPosCheck;

    int kernelIndex;

    ComputeBuffer vertexBuffer; // Declare the buffer as a class member
    ComputeBuffer originalVertexBuffer; // Declare the buffer to store original position as a class member

    // Define the Vertex struct inside the RaiseMesh class

    void Start()
    {
        Matrix4x4 modelToWorldMatrix = transform.localToWorldMatrix;
        kernelIndex = raiseShader.FindKernel("RaiseVertices"); // Find the index of the kernel with the specified name

        raiseShader.SetMatrix("UNITY_MATRIX_MVP", modelToWorldMatrix);

        mf = GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();
        //The Mesh
        mesh = mf.mesh;
        mesh.name += "_Async";
        //if mesh collider is avalible reference it in code and set boolean updateMeshCollider
        //updateMeshCollider = TryGetComponent(out meshCollider);

        InitializeOriginalPosition();
        UpdateVertices();
    }

    void Update()
    {
        if (raiseAmountCheck != raiseAmount || playerPosCheck != playerPos.position)
        {
            UpdateVertices();
            raiseAmountCheck = raiseAmount;
            playerPosCheck = playerPos.position;
        }
    }

    void InitializeOriginalPosition()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        int vertexCount = mesh.vertexCount;

        // Dispose of the buffer from the previous frame
        originalVertexBuffer?.Dispose();

        //the buffer needs to have the same amount of strides as the variables stored in the raiseShader
        originalVertexBuffer = new ComputeBuffer(vertexCount, sizeof(float) * 3);
        originalVertexBuffer.SetData(mesh.vertices);

        int kernelIndex = initializeShader.FindKernel("InitializeOriginalPosition"); // Find the index of the kernel with the specified name
        initializeShader.SetBuffer(kernelIndex, "vertices", originalVertexBuffer);

        initializeShader.Dispatch(kernelIndex, vertexCount / 4, 1, 1);
    }

    void UpdateVertices()
    {
        Mesh mesh = mf.mesh;
        int vertexCount = mesh.vertexCount;

        // Dispose of the buffer from the previous frame
        vertexBuffer?.Dispose();

        if (!mesh) return;

        // Create the vertex buffer with the correct size
        vertexBuffer = new ComputeBuffer(vertexCount, sizeof(float) * 3);
        vertexBuffer.SetData(mesh.vertices);

        raiseShader.SetBuffer(kernelIndex, "vertices", vertexBuffer);
        raiseShader.SetBuffer(kernelIndex, "originalVertices", originalVertexBuffer);
        raiseShader.SetFloat("raiseAmount", raiseAmount);
        float[] vectorToFloat = { playerPos.position.x, playerPos.position.y, playerPos.position.z };
        raiseShader.SetFloats("reverencePosition", vectorToFloat);

        raiseShader.Dispatch(kernelIndex, vertexCount / 4, 1, 1);

        float3[] vertices = new float3[vertexCount];
        vertexBuffer.GetData(vertices);


        //Update to collider
        mesh.MarkDynamic();
        mc.sharedMesh = mesh;
        mesh.vertices = vertices.Select(v => new Vector3(v.x, v.y, v.z)).ToArray();//.Select(v => v.position).ToArray();
        mesh.RecalculateBounds();
        //Update mesh
        //mesh.SetVertices(vertData);
        mesh.RecalculateNormals();
    }

    void OnDestroy()
    {
        // Dispose of the buffer when the script is destroyed
        vertexBuffer?.Dispose();
        originalVertexBuffer?.Dispose();

        if (vertexBuffer != null)
        {
            vertexBuffer.Release();
            vertexBuffer = null;
        }
        if (originalVertexBuffer != null)
        {
            originalVertexBuffer.Release();
            originalVertexBuffer = null;
        }
    }
}