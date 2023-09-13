using UnityEngine;

namespace WorldBuilder.Client.Game.Common.Coordinate
{
    public readonly struct CartesianCoordinate : ICoordinate
    {
        private readonly Vector3 _vector3;

        public float X => _vector3.x;
        public float Y => _vector3.y;
        public float Z => _vector3.z;
        public float Magnitude => _vector3.magnitude;

        public CartesianCoordinate(float x, float y, float z)
        {
            _vector3 = new Vector3(x, y, z);
        }
        
        public CartesianCoordinate(Vector3 vector3)
        {
            _vector3 = vector3;
        }

        public Vector3 ToVector3()
        {
            return _vector3;
        }

        public CartesianCoordinate ToCartesian()
        {
            return this;
        }

        public PolarCoordinate ToPolar()
        {
            var r = _vector3.magnitude;
            var azimuthal = Mathf.Atan2(Y, X);
            var polar = Mathf.Acos(Z / r); 
            return PolarCoordinate.FromRadians(r, polar, azimuthal);
        }
    }
}