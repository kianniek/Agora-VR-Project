using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace AmazingAssets.VertexThicknessGenerator.Example
{
    public class Runtime : MonoBehaviour
    {
        public bool useUniversalCamera = false; //v2023.1 introduced new VTG calculation methed that doesn't use UniversalCameraData. 


        public float rayLength = 1;
        [Range(0.0f, 1.0f)] public float details = 0.7f;

        public Material material;


        //Setup URP camera used in VTG calculations. 
        //Note, v2023.1 introduced new VTG calculation methed that doesn't use UniversalCameraData. 
        void SetupUniversalCameraData(Camera camera)
        {
            //Disabling all URP camera effects
            UniversalAdditionalCameraData cameraData = camera.gameObject.AddComponent<UniversalAdditionalCameraData>();
            cameraData.renderPostProcessing = false;
            cameraData.renderShadows = false;
            cameraData.antialiasing = AntialiasingMode.None;
            cameraData.volumeLayerMask = 0;
            cameraData.requiresDepthTexture = false;


            //Set index of the 'Vertex Thickness Renderer' to be the same as in SRP asset.
            cameraData.SetRenderer(1);
        }


        void Start()
        {
            //Calculated VT will be saved here
            float[] thicknessValues = null;

            if(useUniversalCamera)
                thicknessValues = this.gameObject.GenerateVertexThickness(rayLength, details, -1, SetupUniversalCameraData);
            else
                thicknessValues = this.gameObject.GenerateVertexThickness(rayLength, details, -1);


            if (thicknessValues != null)
            {
                //Saving thickness values inside vertex color
                Color[] vertexColor = new Color[thicknessValues.Length];
                for (int i = 0; i < thicknessValues.Length; i++)
                {
                    vertexColor[i] = Color.Lerp(Color.black, Color.white * thicknessValues[i], thicknessValues[i]);
                }


                //Instantiate copy of a 'mesh' and assign new vertex colors
                this.gameObject.GetComponent<MeshFilter>().mesh.colors = vertexColor;

                //Assign material with vertex color support
                this.gameObject.GetComponent<Renderer>().material = material;
            }
        }        
    }
}