#nullable enable
using Game.Geometry.Coordinate._3D;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Geometry.Sphere
{
    public readonly struct SphereSurfaceCoordinate
    {
        public SphereSurfaceCoordinate(float height, Angle polar, Angle azimuthal)
        {
            Height = height;
            Polar = polar;
            Azimuthal = azimuthal;
        }

        /// <summary>
        ///     This height is relative to the radius of the sphere
        /// </summary>
        public float Height { get; }

        public Angle Polar { get; }
        public Angle Azimuthal { get; }

        public Vector3 ToGlobalCartesianVec3(ISphere sphere)
        {
            var local = ToPolar3D(sphere).ToCartesian();
            return local.ToVector3() + sphere.Center.ToVector3();
        }

        public PolarCoordinate3D ToPolar3D(ISphere sphere)
        {
            return new PolarCoordinate3D(sphere.Radius + Height, Polar, Azimuthal);
        }

        public static SphereSurfaceCoordinate FromPolarCoordinate(PolarCoordinate3D polarCoordinate, ISphere sphere)
        {
            return FromPolarCoordinate(polarCoordinate, sphere.Radius);
        }

        public static SphereSurfaceCoordinate FromPolarCoordinate(PolarCoordinate3D polarCoordinate, float radius)
        {
            var height = polarCoordinate.Radius - radius;
            return new SphereSurfaceCoordinate(height, polarCoordinate.Polar, polarCoordinate.Azimuthal);
        }

        public static SphereSurfaceCoordinate FromLocalVec3(Vector3 globalPosition, float radius)
        {
            var r = globalPosition.magnitude;
            var azimuthal = Mathf.Atan2(globalPosition.y, globalPosition.x);
            var polar = Mathf.Acos(globalPosition.z / r);
            return new SphereSurfaceCoordinate(r - radius, polar.AsRadians(), azimuthal.AsRadians());
        }
    }
}