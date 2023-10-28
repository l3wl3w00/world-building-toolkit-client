#nullable enable
using UnityEngine.UIElements;

namespace Common.Model
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
    }
}