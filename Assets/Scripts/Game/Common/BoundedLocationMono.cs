#nullable enable
using Common;
using Common.Geometry.Projection;
using Common.Geometry.Sphere;
using Common.Utils;
using Game.Continent_;
using Game.Region_;
using ProceduralToolkit;
using UnityEngine;

namespace Game.Common
{
    public abstract partial class BoundedLocationMono : MonoBehaviour
    {
        public bool LinesVisible 
        { 
            get => SphereLineRendererHandler.LinesVisible;
            set => SphereLineRendererHandler.LinesVisible = value;
        }

        public void UpdateLines()
        {
            SphereControlPointsHandler.ReCreateGlobalPoints();
            SphereLineRendererHandler.UpdateLineRenderer();
        }

        public PolygonOnSphere<StereographicProjector> CreatePolygonOnSphere() => PartialSphereMeshHandler.CreatePolygonOnSphere();
    }
    
    public partial class BoundedLocationMono
    {
        private Option<SphereControlPointsHandler> _continentControlPointHandler = Option<SphereControlPointsHandler>.None;
        private Option<SphereLineRendererHandler> _continentLineHandler = Option<SphereLineRendererHandler>.None;
        private Option<PartialSphereMeshHandler> _continentMeshHandler = Option<PartialSphereMeshHandler>.None;
        
        protected SphereLineRendererHandler SphereLineRendererHandler => 
            this.LazyInitialize(ref _continentLineHandler);
        protected PartialSphereMeshHandler PartialSphereMeshHandler => 
            this.LazyInitialize(ref _continentMeshHandler);
        protected SphereControlPointsHandler SphereControlPointsHandler =>
            this.LazyInitialize(ref _continentControlPointHandler);
    }
}