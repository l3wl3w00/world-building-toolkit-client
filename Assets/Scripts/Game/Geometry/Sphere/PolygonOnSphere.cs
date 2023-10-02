using System;
using System.Collections.Generic;
using System.Linq;
using Game.Projection;
using UnityEngine;

namespace Game.Geometry.Sphere
{
    public class PolygonOnSphere : IDisposable
    {
        private readonly PolygonCollider2D _projection2D;
        private readonly ISpherePlaneProjector _spherePlaneProjector;

        public PolygonOnSphere(ISpherePlaneProjector spherePlaneProjector, List<SphereSurfaceCoordinate> coordinates)
        {
            _spherePlaneProjector = spherePlaneProjector;

            _projection2D = new GameObject().AddComponent<PolygonCollider2D>();
            _projection2D.points = coordinates
                .Select(_spherePlaneProjector.SphereToPlane)
                .Select(p => p.ToVector2())
                .ToArray();
        }

        public void Dispose()
        {
            // Object.Destroy(_projection2D.gameObject);
        }

        public bool Contains(TriangleOnSphere triangle)
        {
            return Contains(triangle.Points.Item1) &&
                   Contains(triangle.Points.Item2) &&
                   Contains(triangle.Points.Item3);
        }

        public bool Contains((Vector3, Vector3, Vector3) triangle)
        {
            return Contains(triangle.Item1) &&
                   Contains(triangle.Item2) &&
                   Contains(triangle.Item3);
        }

        private bool Contains(SphereSurfaceCoordinate coordinate)
        {
            var pointOnPlane = _spherePlaneProjector.SphereToPlane(coordinate);
            return _projection2D.OverlapPoint(pointOnPlane.ToVector2());
        }

        private bool Contains(Vector3 coordinate)
        {
            var pointOnPlane = _spherePlaneProjector.SphereToPlane(coordinate);
            return _projection2D.OverlapPoint(pointOnPlane);
        }


        public void DrawCollider()
        {
            if (_projection2D == null) return;

            var offset = _projection2D.offset;
            var trans = _projection2D.transform;

            for (var i = 0; i < _projection2D.pathCount; i++)
            {
                var path = _projection2D.GetPath(i);
                for (var j = 0; j < path.Length; j++)
                {
                    Vector2 start = trans.TransformPoint(path[j] + offset);
                    Vector2 end = trans.TransformPoint(path[(j + 1) % path.Length] + offset);
                    Debug.DrawLine(start, end, Color.red);
                }
            }
        }
    }
}