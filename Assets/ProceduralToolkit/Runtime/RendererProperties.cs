#nullable enable
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralToolkit
{
    /// <summary>
    ///     Serializable Renderer properties
    /// </summary>
    [Serializable]
    public class RendererProperties
    {
        #region Serialized Fields

        public LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes;
        public GameObject lightProbeProxyVolumeOverride;
        public ReflectionProbeUsage reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
        public Transform probeAnchor;
        public ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;
        public bool receiveShadows = true;
        public MotionVectorGenerationMode motionVectorGenerationMode = MotionVectorGenerationMode.Object;

        #endregion
    }
}