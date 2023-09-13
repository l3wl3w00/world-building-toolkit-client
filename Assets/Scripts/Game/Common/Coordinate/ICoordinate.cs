using UnityEngine;

namespace WorldBuilder.Client.Game.Common.Coordinate
{
    public interface ICoordinate
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
        CartesianCoordinate ToCartesian();
        PolarCoordinate ToPolar();
    }
}