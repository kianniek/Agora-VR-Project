using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Collections;
using Unity.Mathematics;

public class RaiseMeshAsync : MonoBehaviour
{
    public ComputeShader raiseShader;
    public ComputeShader initializeShader;

    public MeshFilter mf;
    public MeshCollider mc;
    private Mesh mesh;

    private ComputeBuffer raiseShaderBuffer;
    private ComputeBuffer initializeShaderBuffer;

    private int _kernelRaiseShader;
    private int _kernelInitializeShader;
    private int dispatchCount = 0;

    public float raiseAmount = 1.0f;
    float raiseAmountCheck;
    public Transform playerPos;
    Vector3 playerPosCheck;

    private NativeArray<Vector3> vertData;
    private AsyncGPUReadbackRequest request;

    private void Start()
    {
        if (!SystemInfo.supportsAsyncGPUReadback) { this.gameObject.SetActive(false); return; }
        CleanUp();
        mf = GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();
        //The Mesh
        mesh = mf.mesh;
        mesh.name += "_Async";

        //Run initialize original position compute shader to get the original vertex data of the mesh into the buffer
        InitializeOriginalPosition();

        RaiseVerticesComputeSetup();

        //Request AsyncReadback
        asyncGPUReadbackCallback -= AsyncGPUReadbackCallback;
        asyncGPUReadbackCallback += AsyncGPUReadbackCallback;
        request = AsyncGPUReadback.Request(raiseShaderBuffer, asyncGPUReadbackCallback);

        // Debugging statements
    }

    void InitializeOriginalPosition()
    {
        _kernelInitializeShader = initializeShader.FindKernel("InitializeOriginalPosition"); // Find the index of the kernel with the specified name

        //compute shader
        uint threadX = 0;
        uint threadY = 0;
        uint threadZ = 0;
        initializeShader.GetKernelThreadGroupSizes(_kernelInitializeShader, out threadX, out threadY, out threadZ);
        int _dispatchCount = Mathf.CeilToInt(mesh.vertexCount / threadX + 1);

        // Init mesh vertex array
        Vector3[] meshVerts = mesh.vertices;
        NativeArray<Vector3> _vertData = new(mesh.vertexCount, Allocator.Temp);

        for (int i = 0; i < mesh.vertexCount; ++i)
        {
            _vertData[i] = meshVerts[i];
        }

        //init compute buffer
        initializeShaderBuffer = new ComputeBuffer(mesh.vertexCount, 12); // 3*4bytes = sizeof(Vector3)
        if (_vertData.IsCreated) initializeShaderBuffer.SetData(_vertData);

        initializeShader.SetBuffer(_kernelInitializeShader, "vertices", initializeShaderBuffer);

        initializeShader.Dispatch(_kernelInitializeShader, _dispatchCount / 4, 1, 1);
        print(initializeShaderBuffer.count);
        print("Done Initialize");
    }

    void RaiseVerticesComputeSetup()
    {
        //compute shader
        _kernelRaiseShader = raiseShader.FindKernel("RaiseVertices");
        uint threadX = 0;
        uint threadY = 0;
        uint threadZ = 0;
        raiseShader.GetKernelThreadGroupSizes(_kernelRaiseShader, out threadX, out threadY, out threadZ);
        dispatchCount = Mathf.CeilToInt(mesh.vertexCount / threadX + 1);

        // Init mesh vertex array
        Vector3[] meshVerts = mesh.vertices;
        vertData = new NativeArray<Vector3>(mesh.vertexCount, Allocator.Temp);
        for (int i = 0; i < mesh.vertexCount; ++i)
        {
            vertData[i] = meshVerts[i];
        }

        //init compute buffer
        raiseShaderBuffer = new ComputeBuffer(mesh.vertexCount, 12); // 3*4bytes = sizeof(Vector3)
        if (vertData.IsCreated) raiseShaderBuffer.SetData(vertData);

        Matrix4x4 modelToWorldMatrix = transform.localToWorldMatrix;
        raiseShader.SetMatrix("UNITY_MATRIX_MVP", modelToWorldMatrix);

        raiseShader.SetBuffer(_kernelRaiseShader, "vertices", raiseShaderBuffer);
        raiseShader.SetBuffer(_kernelRaiseShader, "originalVertices", initializeShaderBuffer);
    }
    void Update()
    {
        //run the compute shader, the position of particles will be updated in GPU
        raiseShader.SetFloat("raiseAmount", raiseAmount);
        float[] vectorToFloat = { playerPos.position.x, playerPos.position.y, playerPos.position.z };
        raiseShader.SetFloats("reverencePosition", vectorToFloat);
        raiseShader.Dispatch(_kernelRaiseShader, dispatchCount, 1, 1);
    }

    //The callback will be run when the request is ready
    private static event System.Action<AsyncGPUReadbackRequest> asyncGPUReadbackCallback;
    public void AsyncGPUReadbackCallback(AsyncGPUReadbackRequest request)
    {
        // Register a callback to process the readback data when it is ready

        if (request.hasError)
        {
            Debug.Log("AsyncGPUReadback error detected");
        }
        else
        {
            if (!mesh) return;
            //Readback and show result on texture
            vertData = request.GetData<Vector3>();
            string result = "[" + string.Join(",", vertData) + "]";
            print(result);
            //Update mesh
            mesh.MarkDynamic();
            mesh.SetVertices(vertData);
            mesh.RecalculateNormals();

            //Update to collider
            mc.sharedMesh = mesh;
        }
        //Request AsyncReadback again
        request = AsyncGPUReadback.Request(raiseShaderBuffer, asyncGPUReadbackCallback);
    }

    private void CleanUp()
    {
        raiseShaderBuffer?.Release();
        initializeShaderBuffer?.Release();
        asyncGPUReadbackCallback -= AsyncGPUReadbackCallback;
    }

    void OnDisable()
    {
        CleanUp();
    }

    void OnDestroy()
    {
        CleanUp();
    }
}
//Around 60 FPS if we do not use AsyncGPUReadback
//Around 145 FPS if we use AsyncGPUReadback!
