using UnityEngine;
using WorldBuilder.Client.Game.Common.Coordinate;

namespace WorldBuilder.Client.Game.Common.Sphere
{        
    public delegate void SphereRotationChanged(Quaternion newRotation);

    public interface ISphere
    {
        ICoordinate Center { get; }
        float Radius { get; }
        Quaternion Rotation { get; }
    }
}