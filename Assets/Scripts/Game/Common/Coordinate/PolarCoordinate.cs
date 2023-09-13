using UnityEngine;
using UnityEngine.UIElements;

namespace WorldBuilder.Client.Game.Common.Coordinate
{
    public readonly struct PolarCoordinate : ICoordinate
    {
        public PolarCoordinate(float radius, Angle polar, Angle azimuthal)
        {
            this.Radius = radius;
            this.Polar = polar;
            this.Azimuthal = azimuthal;
        }

        public float Radius { get; }
        public Angle Polar { get; }
        public Angle Azimuthal { get; }

        public static PolarCoordinate FromRadians(float radius, float polar, float azimuthal)
        {
            return FromRadians(new Vector3(radius, polar, azimuthal));
        }

        public static PolarCoordinate FromRadians(Vector3 vector3)
        {
            return new PolarCoordinate(vector3.x, Angle.Radians(vector3.y), Angle.Radians(vector3.z));
        }

        public Vector3 ToVector3()
        {
            return new Vector3(Radius, Polar.ToRadians(), Azimuthal.ToRadians());
        }

        public CartesianCoordinate ToCartesian()
        {
            
            var x = Radius * Mathf.Sin(Polar.ToRadians()) * Mathf.Cos(Azimuthal.ToRadians());
            var y = Radius * Mathf.Sin(Polar.ToRadians()) * Mathf.Sin(Azimuthal.ToRadians());
            var z = Radius * Mathf.Cos(Polar.ToRadians());

            return new CartesianCoordinate(x, y, z);
        }

        public PolarCoordinate ToPolar()
        {
            return this;
        }
    }
}