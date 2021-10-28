using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class KawaseRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class FeatureSettings
    {
        public string passName = "Kawase";
        public RenderPassEvent passEvent = RenderPassEvent.AfterRenderingOpaques;
        
        public Material material;

        [Range(0, 1)] public float lerp;
        
    }

    private FeatureSettings m_Settings = new FeatureSettings();

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        
    }

    public override void Create()
    {
        
    }
}
