using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TrampleObject : MonoBehaviour
{
    [SerializeField] private UniversalRendererData renderSettings = null;

    private bool tryGetFeature(out ShaderRender feature)
    {
        feature = renderSettings.rendererFeatures.OfType<ShaderRender>().FirstOrDefault();
        return feature != null; 
    }
}
