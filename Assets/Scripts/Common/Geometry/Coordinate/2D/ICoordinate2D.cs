#nullable enable
using UnityEngine;

namespace Common.Geometry.Coordinate._2D
{
    public interface ICoordinate2D
    {
        Vector2 ToVector2();

        Vector2 ToCartesianVector2()
        {
            return ToCartesian().ToVector2();
        }

        // Vector2 ToPolarVector3()
        // {
        //     return ToPolar().ToVector2();
        // }

        // interface depends on implementations because not many more implementations of Coordinate are expected
        CartesianCoordinate2D ToCartesian();
    }
}