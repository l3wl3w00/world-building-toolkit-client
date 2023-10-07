#nullable enable
using System.Linq;
using Game.Geometry.Sphere;
using Game.Projection;
using Game.Util;
using ProceduralToolkit;
using UnityEngine;

namespace Game.Continent
{
    internal class ContinentMeshHandler : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        // private PolygonOnSphere? __debugPolygonOnSphere;
        private Option<ContinentControlPointHandler> _continentControlPointHandler = Option<ContinentControlPointHandler>.None;
        private Option<MeshCollider> _meshCollider = Option<MeshCollider>.None;
        private Option<MeshFilter> _meshFilter = Option<MeshFilter>.None;
        internal bool Invert { get; set; }

        private ContinentControlPointHandler ContinentControlPointHandler =>
            this.LazyInitialize(ref _continentControlPointHandler);

        private MeshFilter MeshFilter => this.LazyInitialize(ref _meshFilter);
        private MeshCollider MeshCollider => this.LazyInitialize(ref _meshCollider);

        #region Event Functions

        private void Update()
        {
            // __debugPolygonOnSphere?.DrawCollider();
        }

        #endregion

        internal void UpdateMesh()
        {
            if (ContinentControlPointHandler.ControlPoints.Count <= 1) return;

            ContinentControlPointHandler.ReCreateGlobalPoints();
            var draft = CreateSphereMeshDraft(700, 700);
            using var polygonOnSphere = CreatePolygonOnSphere();
            // __debugPolygonOnSphere = polygonOnSphere;
            RemoveAllNotContainedTriangles(draft, polygonOnSphere);
            // polygonOnSphere.DrawCollider();
            var mesh = draft.ToMesh();
            mesh.RecalculateNormals();
            MeshFilter.mesh = mesh;
            MeshCollider.sharedMesh = mesh;
        }

        private MeshDraft CreateSphereMeshDraft(int horizontalSegments, int verticalSegments)
        {
            var result = MeshDraft.Sphere(1, horizontalSegments, verticalSegments);

            var trianglesMultipleOf3 = result.triangles.Count % 3 == 0;
            if (!trianglesMultipleOf3) Debug.LogError("triangle count in MeshDraft is not divisible by 3!");

            return result;
        }


        private PolygonOnSphere CreatePolygonOnSphere()
        {
            var sphereSurfaceCoordinates = ContinentControlPointHandler.DetailedGlobalPoints
                .Select(p => Quaternion.Inverse(ContinentControlPointHandler.PlanetControl.Rotation) * p)
                .Select(c =>
                    SphereSurfaceCoordinate.FromLocalVec3(c, ContinentControlPointHandler.PlanetControl.Radius))
                .ToList();
            return new PolygonOnSphere(new StereographicProjector(ContinentControlPointHandler.PlanetControl),
                sphereSurfaceCoordinates);
        }

        private void RemoveAllNotContainedTriangles(MeshDraft meshDraft, PolygonOnSphere polygonOnSphere)
        {
            // must use traditional for loop or else this is very slow
            for (var i = 0; i < meshDraft.triangles.Count; i += 3)
            {
                var vertices = GetVertices(meshDraft, i + 0, i + 1, i + 2);

                if (PolygonContains(vertices)) continue;

                FlagForDeletion(i + 0);
                FlagForDeletion(i + 1);
                FlagForDeletion(i + 2);
            }

            meshDraft.triangles.RemoveAll(IsFlaggedForDeletion);
            return;

            void FlagForDeletion(int i)
            {
                meshDraft.triangles[i] = -1;
            }

            bool IsFlaggedForDeletion(int triangle)
            {
                return triangle == -1;
            }

            bool PolygonContains((Vector3, Vector3, Vector3) vertices)
            {
                var contains = polygonOnSphere.Contains(vertices);
                if (Invert) return !contains;
                return contains;
            }
        }


        private (Vector3, Vector3, Vector3) GetVertices(
            MeshDraft meshDraft,
            int indexIntoTriangles0,
            int indexIntoTriangles1,
            int indexIntoTriangles2)
        {
            var v1Index = meshDraft.triangles[indexIntoTriangles0];
            var v2Index = meshDraft.triangles[indexIntoTriangles1];
            var v3Index = meshDraft.triangles[indexIntoTriangles2];

            var v1 = meshDraft.vertices[v1Index];
            var v2 = meshDraft.vertices[v2Index];
            var v3 = meshDraft.vertices[v3Index];
            return (v1, v2, v3);
        }
    }
}