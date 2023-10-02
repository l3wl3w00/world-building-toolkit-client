#nullable enable
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Geometry.Coordinate._3D
{
    public static class CoordinateExtensions
    {
        public static CartesianCoordinate3D ToCartesian(this Vector3 vector3)
        {
            return new CartesianCoordinate3D(vector3);
        }

        public static Angle AsRadians(this float alpha)
        {
            return Angle.Radians(alpha);
        }
    }
}