#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Geometry.Projection;
using Common.Model;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Common.Geometry.Sphere
{
    public interface IPolygonOnSphere: IDisposable
    {
        bool Contains(Vector3 v1, Vector3 v2, Vector3 v3);
    }

    public struct InfinitePolygonOnSphere : IPolygonOnSphere
    {
        public void Dispose() { }
        public bool Contains(Vector3 v1, Vector3 v2, Vector3 v3) => true;
    }

    public readonly struct PolygonOnSphere<TProjector> where TProjector : ISpherePlaneProjector, new()
    {
        private readonly PolygonCollider2D _projection2D;
        private readonly TProjector _spherePlaneProjector;
        private readonly bool _isInfinite;

        public PolygonOnSphere(TProjector projector, List<SphereSurfaceCoordinate> coordinates, bool isInfinite = false)
        {
            _spherePlaneProjector = projector;
            _isInfinite = isInfinite;

            _projection2D = new GameObject().AddComponent<PolygonCollider2D>();
            _projection2D.points = coordinates
                .Select(_spherePlaneProjector.SphereToPlane)
                .Select(p => p.ToVector2())
                .ToArray();
        }

        public static PolygonOnSphere<TProjector> Infinite() => new(new(),new(),true);

        public void Dispose()
        {
            Object.Destroy(_projection2D.gameObject);
        }
        
        public bool Contains(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            if (_isInfinite) return true;
            var v1OnPlane = _spherePlaneProjector.SphereToPlane(v1);
            var v2OnPlane = _spherePlaneProjector.SphereToPlane(v2);
            var v3OnPlane = _spherePlaneProjector.SphereToPlane(v3);
            
            return _projection2D.OverlapPoint(v1OnPlane) &&
                   _projection2D.OverlapPoint(v2OnPlane) &&
                   _projection2D.OverlapPoint(v3OnPlane);
        }

        public unsafe Vector2 SphereToPlaneUnsafe(Vector3* v) => _spherePlaneProjector.SphereToPlaneUnsafe(v);
        
        public unsafe bool ContainsUnsafe((Vector3 v1, Vector3 v2, Vector3 v3)* vertices)
        {
            if (_isInfinite) return true;
            var v1OnPlane = _spherePlaneProjector.SphereToPlaneUnsafe(&vertices->v1);
            var v2OnPlane = _spherePlaneProjector.SphereToPlaneUnsafe(&vertices->v2);
            var v3OnPlane = _spherePlaneProjector.SphereToPlaneUnsafe(&vertices->v3);
            
            return _projection2D.OverlapPoint(v1OnPlane) &&
                   _projection2D.OverlapPoint(v2OnPlane) &&
                   _projection2D.OverlapPoint(v3OnPlane);
        }

        public void DrawCollider()
        {
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