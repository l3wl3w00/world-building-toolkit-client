#nullable enable
using System;
using Common.Geometry.Coordinate._2D;
using Common.Geometry.Sphere;
using Common.Model;
using UnityEngine;

namespace Common.Geometry.Projection
{
    public readonly struct StereographicProjector : ISpherePlaneProjector
    {
        private static int _counter;

        public ISphere Sphere { get; }

        public StereographicProjector(ISphere sphere)
        {
            Sphere = sphere;
        }

        public CartesianCoordinate2D SphereToPlane(SphereSurfaceCoordinate sphereSurfaceCoordinate)
        {
            var globalPosition = sphereSurfaceCoordinate.ToGlobalCartesianVec3(Sphere);

            return new CartesianCoordinate2D(SphereToPlane(globalPosition));
        }

        public Vector2 SphereToPlane(Vector3 cartesian)
        {
            var cartesianNormalized = cartesian.normalized;
            var x = cartesianNormalized.x;
            var y = cartesianNormalized.y;
            var z = cartesianNormalized.z;
            // if (Math.Abs(y - 1f) < 0.0001f)
            // {
            //     y = 0.9999f;
            // }
            // var result = new Vector2(x / (1f - z), y / (1f - z));
            var result = new Vector2(x / (1f - y), z / (1f - y));


            // var x = cartesian.x;
            // var y = cartesian.y;
            // var z = cartesian.z;
            // var r = cartesian.magnitude;
            // var result = new Vector2(r * x / (r - y), r * z / (r - y));
            // DebugDrawLine(cartesian, result);
            return result;
        }

        public unsafe Vector2 SphereToPlaneUnsafe(Vector3* cartesian)
        {
            var cartesianNormalized = cartesian->normalized;
            var x = cartesianNormalized.x;
            var y = cartesianNormalized.y;
            var z = cartesianNormalized.z;
            // if (Math.Abs(y - 1f) < 0.0001f)
            // {
            //     y = 0.9999f;
            // }
            // var result = new Vector2(x / (100f - z), y / (100f - z));
            var result = new Vector2(x / (1f - y), z / (1f - y));
            return result;
        }

        public SphereSurfaceCoordinate PlaneToSphere(CartesianCoordinate2D sphereSurfaceCoordinate)
        {
            throw new NotImplementedException();
        }

        private void DebugDrawLine(Vector3 from, Vector2 to)
        {
            _counter++;
            if (_counter % 100 != 0) return;
            var lineRenderer = new GameObject().AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.5f;
            lineRenderer.endWidth = 0.01f;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, from);
            lineRenderer.SetPosition(1, new Vector3(to.x, to.y, 0));
        }
    }
}