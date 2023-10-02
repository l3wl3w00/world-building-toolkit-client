using System;
using UnityEngine;

namespace Game.Geometry.Coordinate._2D
{
    public struct CartesianCoordinate2D : ICoordinate2D
    {
        public float Y { get; }
        public float X { get; }

        public CartesianCoordinate2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public CartesianCoordinate2D(Vector2 sphereToPlane)
        {
            X = sphereToPlane.x;
            Y = sphereToPlane.y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public CartesianCoordinate2D ToCartesian()
        {
            throw new NotImplementedException();
        }
    }
}