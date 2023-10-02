#nullable enable
using System.Collections.Generic;
using Game.Camera_;
using UnityEngine;

namespace Game.Continent
{
    internal class ContinentLineRendererHandler : MonoBehaviour
    {
        private const float LineWidthScale = 0.02f;

        private Camera _camera;
        private ContinentControlPointHandler _continentControlPointHandler;
        private LineRenderer _lineRenderer;
        private LineRenderer LineRenderer => LazyInitialize(ref _lineRenderer);

        private List<Vector3> DetailedGlobalPoints =>
            LazyInitialize(ref _continentControlPointHandler).DetailedGlobalPoints;

        #region Event Functions

        private void Update()
        {
            var cameraPlanetEdgeDistance = transform.EdgeDistanceFromCamera(_camera);

            LineRenderer.endWidth = cameraPlanetEdgeDistance * LineWidthScale;
            LineRenderer.startWidth = cameraPlanetEdgeDistance * LineWidthScale;
        }

        #endregion

        internal void Initialize(Camera initialCamera)
        {
            _camera = initialCamera;
        }

        internal void UpdateLineRenderer()
        {
            LineRenderer.positionCount = DetailedGlobalPoints.Count;
            LineRenderer.SetPositions(DetailedGlobalPoints.ToArray());
        }

        private T LazyInitialize<T>(ref T component) where T : Component
        {
            if (component != null) return component;
            component = GetComponent<T>();
            return component;
        }

        public void DeleteLines()
        {
            LineRenderer.positionCount = 0;
        }
    }
}