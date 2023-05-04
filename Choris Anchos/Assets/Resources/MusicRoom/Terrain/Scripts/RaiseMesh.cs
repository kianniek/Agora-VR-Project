using System.Linq;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

public class RaiseMesh : MonoBehaviour
{
    MeshFilter meshFilter;
    public ComputeShader raiseShader;
    public ComputeShader initializeShader;
    public float raiseAmount = 1.0f;
    public Transform playerPos;
    Vector3 playerPosCheck;
    float raiseAmountCheck;

    [Tooltip("If enabled, the mesh collider on the object will also be updated")]
    //[SerializeField] bool updateMeshCollider;

    ComputeBuffer vertexBuffer; // Declare the buffer as a class member
    ComputeBuffer originalVertexBuffer; // Declare the buffer to store original position as a class member

    // Define the Vertex struct inside the RaiseMesh class
    struct Vertex
    {
        public Vector3 position;
    }

    void Start()
    {
        Matrix4x4 modelToWorldMatrix = transform.localToWorldMatrix;


        raiseShader.SetMatrix("UNITY_MATRIX_MVP", modelToWorldMatrix);

        meshFilter = GetComponent<MeshFilter>();
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
            UpdateMeshCollider();
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

        //the buffer needs to have the same amount of strides as the variables stored in the computeShader
        originalVertexBuffer = new ComputeBuffer(vertexCount, sizeof(float) * 3);
        originalVertexBuffer.SetData(mesh.vertices);

        int kernelIndex = initializeShader.FindKernel("InitializeOriginalPosition"); // Find the index of the kernel with the specified name
        initializeShader.SetBuffer(kernelIndex, "vertices", originalVertexBuffer);

        initializeShader.Dispatch(kernelIndex, vertexCount / 4, 1, 1);
    }

    void UpdateVertices()
    {
        Mesh mesh = meshFilter.mesh;
        int vertexCount = mesh.vertexCount;

        // Dispose of the buffer from the previous frame
        vertexBuffer?.Dispose();

        // Create the vertex buffer with the correct size
        vertexBuffer = new ComputeBuffer(vertexCount, sizeof(float) * 3);
        vertexBuffer.SetData(mesh.vertices);

        int kernelIndex = raiseShader.FindKernel("RaiseVertices"); // Find the index of the kernel with the specified name


        raiseShader.SetBuffer(kernelIndex, "vertices", vertexBuffer);
        raiseShader.SetBuffer(kernelIndex, "originalVertices", originalVertexBuffer);
        raiseShader.SetFloat("raiseAmount", raiseAmount);
        float[] vectorToFloat = { playerPos.position.x, playerPos.position.y, playerPos.position.z };
        raiseShader.SetFloats("reverencePosition", vectorToFloat);

        raiseShader.Dispatch(kernelIndex, vertexCount / 4, 1, 1);

        Vertex[] vertices = new Vertex[vertexCount];
        vertexBuffer.GetData(vertices);

        mesh.vertices = vertices.Select(v => v.position).ToArray();
        mesh.RecalculateBounds();
    }

    void UpdateMeshCollider()
    {
        //if (updateMeshCollider) { return; }

    }   

    void OnDestroy()
    {
        // Dispose of the buffer when the script is destroyed
        vertexBuffer?.Dispose();
        originalVertexBuffer?.Dispose();
    }

    void Cleanup()
    {
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
