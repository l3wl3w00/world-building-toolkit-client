#nullable enable
using System.Collections.Generic;
using Common.Geometry.Coordinate._3D;
using UnityEngine;

namespace Common.Geometry.Sphere
{
    public delegate void SphereRotationChanged(Quaternion newRotation);

    public interface ISphere
    {
        ICoordinate3D Center => Transform.position.ToCartesian();
        float Radius => Transform.localScale.x;
        Quaternion Rotation => Transform.rotation;
        Transform Transform { get; }
        
        float EdgeDistanceFromCamera { get; }
    }
}