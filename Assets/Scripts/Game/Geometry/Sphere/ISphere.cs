#nullable enable
using System.Collections.Generic;
using Game.Geometry.Coordinate._3D;
using UnityEngine;

namespace Game.Geometry.Sphere
{
    public delegate void SphereRotationChanged(Quaternion newRotation);

    public interface ISphere
    {
        ICoordinate3D Center { get; }
        float Radius { get; }
        Quaternion Rotation { get; }

        IEnumerable<(int, int, int)> Triangles(int verticalResolution, int horizontalResolution, float height,
            List<Vector3> vertices);
    }
}