#nullable enable
using System.Collections.Generic;
using Common.Model;
using UnityEngine;

namespace Common.Geometry.Sphere
{
    public readonly struct TriangleOnSphere
    {
        public TriangleOnSphere((SphereSurfaceCoordinate, SphereSurfaceCoordinate, SphereSurfaceCoordinate) points)
        {
            Points = points;
        }

        public TriangleOnSphere(SphereSurfaceCoordinate p1, SphereSurfaceCoordinate p2, SphereSurfaceCoordinate p3)
        {
            Points = (p1, p2, p3);
        }

        public (SphereSurfaceCoordinate, SphereSurfaceCoordinate, SphereSurfaceCoordinate) Points { get; }

        public void AddToMesh(List<Vector3> vertices, List<int> triangles, ISphere sphere)
        {
            HandlePoint(Points.Item1);
            HandlePoint(Points.Item2);
            HandlePoint(Points.Item3);
            return;

            void HandlePoint(SphereSurfaceCoordinate sphereSurfaceCoordinate)
            {
                var vertexAsVec3 = sphereSurfaceCoordinate.ToGlobalCartesianVec3(sphere);
                var index = IndexOfVertex(vertexAsVec3);
                var vertexAlreadyInMesh = index != -1;

                if (vertexAlreadyInMesh)
                {
                    triangles.Add(index);
                    return;
                }

                vertices.Add(vertexAsVec3);
                triangles.Add(vertices.Count - 1);
            }

            int IndexOfVertex(Vector3 vertex)
            {
                const float delta = 0.00001f;
                var index = vertices.FindIndex(v => Vector3.Distance(v, vertex) < delta);
                return index;
            }
        }
    }
}