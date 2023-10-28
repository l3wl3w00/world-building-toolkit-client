#nullable enable
using Common.Geometry.Coordinate._3D;
using Common.Model;
using UnityEngine;

namespace Common.Geometry.Sphere
{
    public static class SphereSurfaceCoordinateUtils
    {

        public static Vector3 ToGlobalCartesianVec3(this SphereSurfaceCoordinate sphereSurfaceCoordinate, ISphere sphere)
        {
            var local = sphereSurfaceCoordinate.ToPolar3D(sphere).ToCartesian();
            return local.ToVector3() + sphere.Center.ToVector3();
        }

        public static PolarCoordinate3D ToPolar3D(this SphereSurfaceCoordinate coordinate, ISphere sphere)
        {
            return new PolarCoordinate3D(sphere.Radius + coordinate.Height, coordinate.Polar, coordinate.Azimuthal);
        }

        public static SphereSurfaceCoordinate ToSphereSurfaceCoordinate(this PolarCoordinate3D polarCoordinate, ISphere sphere)
        {
            return polarCoordinate.ToSphereSurfaceCoordinate(sphere.Radius);
        }

        public static SphereSurfaceCoordinate ToSphereSurfaceCoordinate(this PolarCoordinate3D polarCoordinate, float radius)
        {
            var height = polarCoordinate.Radius - radius;
            return new SphereSurfaceCoordinate(height, polarCoordinate.Polar, polarCoordinate.Azimuthal);
        }

        public static SphereSurfaceCoordinate ToSphereSurfaceCoordinate(this Vector3 globalPosition, float radius)
        {
            var r = globalPosition.magnitude;
            var azimuthal = Mathf.Atan2(globalPosition.y, globalPosition.x);
            var polar = Mathf.Acos(globalPosition.z / r);
            return new SphereSurfaceCoordinate(r - radius, polar.AsRadians(), azimuthal.AsRadians());
        }
    }
}