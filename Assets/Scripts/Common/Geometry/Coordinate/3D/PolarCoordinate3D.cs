#nullable enable
using UnityEngine;
using UnityEngine.UIElements;

namespace Common.Geometry.Coordinate._3D
{
    public readonly struct PolarCoordinate3D : ICoordinate3D
    {
        public PolarCoordinate3D(float radius, Angle polar, Angle azimuthal)
        {
            Radius = radius;
            Polar = polar;
            Azimuthal = azimuthal;
        }

        public float Radius { get; }
        public Angle Polar { get; }
        public Angle Azimuthal { get; }

        public static PolarCoordinate3D FromRadians(float radius, float polar, float azimuthal)
        {
            return FromRadians(new Vector3(radius, polar, azimuthal));
        }

        public static PolarCoordinate3D FromRadians(Vector3 vector3)
        {
            return new PolarCoordinate3D(vector3.x, Angle.Radians(vector3.y), Angle.Radians(vector3.z));
        }

        public Vector3 ToVector3()
        {
            return new Vector3(Radius, Polar.ToRadians(), Azimuthal.ToRadians());
        }

        public CartesianCoordinate3D ToCartesian()
        {
            var x = Radius * Mathf.Sin(Polar.ToRadians()) * Mathf.Cos(Azimuthal.ToRadians());
            var y = Radius * Mathf.Sin(Polar.ToRadians()) * Mathf.Sin(Azimuthal.ToRadians());
            var z = Radius * Mathf.Cos(Polar.ToRadians());

            return new CartesianCoordinate3D(x, y, z);
        }

        public PolarCoordinate3D ToPolar()
        {
            return this;
        }
    }
}