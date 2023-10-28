#nullable enable
using System.Collections.Generic;
using Common;
using Common.Geometry.Sphere;
using Common.Utils;
using UnityEngine;

namespace Game.Common
{
    public class SphereLineRendererHandler : MonoBehaviour
    {
        private const float LineWidthScale = 0.01f;
        
        private ISphere _sphere = null!; //Asserted in Start
        private Option<SphereControlPointsHandler> _continentControlPointHandler = Option<SphereControlPointsHandler>.None;
        private Option<LineRenderer> _lineRenderer = Option<LineRenderer>.None;
        private LineRenderer LineRenderer => this.LazyInitialize(ref _lineRenderer);

        private List<Vector3> DetailedGlobalPoints =>
            this.LazyInitialize(ref _continentControlPointHandler).DetailedGlobalPoints;

        public bool LinesVisible { get => LineRenderer.enabled;  set => LineRenderer.enabled = value; }

        #region Event Functions

        private void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _sphere);
        }

        private void Update()
        {
            var cameraPlanetEdgeDistance = _sphere.EdgeDistanceFromCamera;

            LineRenderer.endWidth = cameraPlanetEdgeDistance * LineWidthScale;
            LineRenderer.startWidth = cameraPlanetEdgeDistance * LineWidthScale;
        }

        #endregion
        
        internal void UpdateLineRenderer()
        {
            LineRenderer.positionCount = DetailedGlobalPoints.Count;
            LineRenderer.SetPositions(DetailedGlobalPoints.ToArray());
        }

        public void Initialize(ISphere sphere)
        {
            this._sphere = sphere;
        }
    }
}