using UnityEngine;

namespace Game.Geometry.Coordinate._3D
{
    public readonly struct CartesianCoordinate3D : ICoordinate3D
    {
        private readonly Vector3 _vector3;

        public float X => _vector3.x;
        public float Y => _vector3.y;
        public float Z => _vector3.z;
        public float Magnitude => _vector3.magnitude;

        public CartesianCoordinate3D(float x, float y, float z)
        {
            _vector3 = new Vector3(x, y, z);
        }

        public CartesianCoordinate3D(Vector3 vector3)
        {
            _vector3 = vector3;
        }

        public Vector3 ToVector3()
        {
            return _vector3;
        }

        public CartesianCoordinate3D ToCartesian()
        {
            return this;
        }

        public PolarCoordinate3D ToPolar()
        {
            var r = _vector3.magnitude;
            var azimuthal = Mathf.Atan2(Y, X);
            var polar = Mathf.Acos(Z / r);
            return PolarCoordinate3D.FromRadians(r, polar, azimuthal);
        }
    }
}