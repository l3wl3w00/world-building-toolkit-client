using UnityEngine;

namespace Game.Geometry.Coordinate._3D
{
    public interface ICoordinate3D
    {
        Vector3 ToVector3();

        Vector3 ToCartesianVector3()
        {
            return ToCartesian().ToVector3();
        }

        Vector3 ToPolarVector3()
        {
            return ToPolar().ToVector3();
        }

        // interface depends on implementations because not many more implementations of Coordinate are expected
        CartesianCoordinate3D ToCartesian();
        PolarCoordinate3D ToPolar();
    }
}