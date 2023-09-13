using UnityEngine;

namespace WorldBuilder.Client.Game.Common.Coordinate
{
    public static class Vector3ToCoordinateExtensions
    {
        public static CartesianCoordinate ToCartesian(this Vector3 vector3)
        {
            return new CartesianCoordinate(vector3);
        }
    }
}