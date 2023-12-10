#nullable enable
using System.Linq;
using Common;
using Common.Geometry.Projection;
using Common.Geometry.Sphere;
using Common.Utils;
using Game.Continent_;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Common
{
    public class PartialSphereMeshHandler : MonoBehaviour
    {
        private Option<SphereControlPointsHandler> _continentControlPointHandler = Option<SphereControlPointsHandler>.None;
        private Option<MeshCollider> _meshCollider = Option<MeshCollider>.None;
        private Option<MeshFilter> _meshFilter = Option<MeshFilter>.None;
        private Option<MeshRenderer> _meshRenderer = Option<MeshRenderer>.None;
        private SphereControlPointsHandler SphereControlPointsHandler =>
            this.LazyInitialize(ref _continentControlPointHandler);

        private MeshFilter MeshFilter => this.LazyInitialize(ref _meshFilter);
        private MeshCollider MeshCollider => this.LazyInitialize(ref _meshCollider);
        private MeshRenderer MeshRenderer => this.LazyInitialize(ref _meshRenderer);

        public void UpdateMesh(Mesh mesh, Color color)
        {
            mesh.RecalculateNormals();
            var material = MeshRenderer.material;
            material.color = color;
            material.renderQueue = (int) RenderQueue.Transparent;
            MeshFilter.mesh = mesh;
        }
        
        public void UpdateMeshCollider(Mesh mesh)
        {
            MeshCollider.sharedMesh = mesh;
        }
        
        public PolygonOnSphere<StereographicProjector> CreatePolygonOnSphere()
        {
            var sphereSurfaceCoordinates = SphereControlPointsHandler.DetailedGlobalPoints
                .Select(p => Quaternion.Inverse(SphereControlPointsHandler.Sphere.Rotation) * p)
                .Select(c =>
                    c.ToSphereSurfaceCoordinate(SphereControlPointsHandler.Sphere.Radius))
                .ToList();
            return new PolygonOnSphere<StereographicProjector>(new StereographicProjector(SphereControlPointsHandler.Sphere), sphereSurfaceCoordinates);
        }
    }
}