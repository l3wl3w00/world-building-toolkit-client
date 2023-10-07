#nullable enable
using System;
using System.Collections.Generic;
using Game.Camera_;
using Game.Util;
using UnityEngine;

namespace Game.Continent
{
    internal class ContinentLineRendererHandler : MonoBehaviour
    {
        private const float LineWidthScale = 0.01f;

        private Option<Camera> _camera = Option<Camera>.None;
        private Option<ContinentControlPointHandler> _continentControlPointHandler = Option<ContinentControlPointHandler>.None;
        private Option<LineRenderer> _lineRenderer = Option<LineRenderer>.None;
        private LineRenderer LineRenderer => this.LazyInitialize(ref _lineRenderer);

        private List<Vector3> DetailedGlobalPoints =>
            this.LazyInitialize(ref _continentControlPointHandler).DetailedGlobalPoints;

        #region Event Functions
        
        private void Update()
        {
            var selfCamera = _camera.ExpectNotNull(nameof(_camera), (Action)Update);
            var cameraPlanetEdgeDistance = transform.EdgeDistanceFromCamera(selfCamera);

            LineRenderer.endWidth = cameraPlanetEdgeDistance * LineWidthScale;
            LineRenderer.startWidth = cameraPlanetEdgeDistance * LineWidthScale;
        }

        #endregion

        internal void Initialize(Camera initialCamera)
        {
            _camera = initialCamera.ToOption();
        }

        internal void UpdateLineRenderer()
        {
            LineRenderer.positionCount = DetailedGlobalPoints.Count;
            LineRenderer.SetPositions(DetailedGlobalPoints.ToArray());
        }
        public void DeleteLines()
        {
            LineRenderer.positionCount = 0;
        }
    }
}