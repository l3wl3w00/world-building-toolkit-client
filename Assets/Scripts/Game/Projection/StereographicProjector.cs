#nullable enable
using System;
using Game.Geometry.Coordinate._2D;
using Game.Geometry.Sphere;
using UnityEngine;

namespace Game.Projection
{
    public class StereographicProjector : ISpherePlaneProjector
    {
        private static int counter;

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
            var result = new Vector2(x / (1f - y), z / (1f - y));
            // var x = cartesian.x;
            // var y = cartesian.y;
            // var z = cartesian.z;
            // var r = cartesian.magnitude;
            // var result = new Vector2(r * x / (r - y), r * z / (r - y));
            // DebugDrawLine(cartesian, result);
            return result;
        }

        public SphereSurfaceCoordinate PlaneToSphere(CartesianCoordinate2D sphereSurfaceCoordinate)
        {
            throw new NotImplementedException();
        }

        private void DebugDrawLine(Vector3 from, Vector2 to)
        {
            counter++;
            if (counter % 100 != 0) return;
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