using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Linq;

public class ShaderRender : ScriptableRendererFeature
{
    class Pass : ScriptableRenderPass
    {
        private Vector4[] tramplePositions;
        private int numTramplePositions;

        public Pass(Vector4[] tramplePositions)
        {
            this.tramplePositions = tramplePositions;
        }
        public int NumTramplePositions { get => this.numTramplePositions; set => this.numTramplePositions = value; }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("ShaderRender");

            cmd.SetGlobalVectorArray("_GrassTramplePositions", tramplePositions);

            cmd.SetGlobalInt("_NumGrassTramplePositions", numTramplePositions);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [SerializeField] private int maxTrackedTransforms = 8;
    private Pass pass;
    private List<Transform> trackingTransforms;
    private Vector4[] tramplePositions;

    public void AddTrackedTransform(Transform transform)
    {
        trackingTransforms.Add(transform);
    }

    public void removeTRackedTransform(Transform transform)
    {
        trackingTransforms.Remove(transform);
    }
    /// <inheritdoc/>
    public override void Create()
    {
        trackingTransforms = new List<Transform>();

        trackingTransforms.AddRange(FindObjectsOfType<TrampleObject>().Select((o) => o.transform));

        tramplePositions = new Vector4[maxTrackedTransforms];
        
        pass = new Pass(tramplePositions);

        // Configures where the render pass should be injected.
        pass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if UNITY_EDITOR

        trackingTransforms.RemoveAll((t) => t == null);
#endif
        for (int i = 0; i < tramplePositions.Length; i++)
        {
            tramplePositions[i] = Vector4.zero;
        }

        int count = (int)Mathf.Min(trackingTransforms.Count, tramplePositions.Length);
        for (int i = 0; i < count; i++)
        {
            Vector3 posn = trackingTransforms[i].position;
            tramplePositions[i] = new Vector4(posn.x, posn.y, posn.z,1);

        }

        pass.NumTramplePositions = count;
        renderer.EnqueuePass(pass);
    }
}


