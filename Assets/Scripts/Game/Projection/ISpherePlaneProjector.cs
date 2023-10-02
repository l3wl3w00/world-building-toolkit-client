#nullable enable
using Game.Geometry.Coordinate._2D;
using Game.Geometry.Sphere;
using UnityEngine;

namespace Game.Projection
{
    public interface ISpherePlaneProjector
    {
        CartesianCoordinate2D SphereToPlane(SphereSurfaceCoordinate sphereSurfaceCoordinate);
        Vector2 SphereToPlane(Vector3 cartesian);
        SphereSurfaceCoordinate PlaneToSphere(CartesianCoordinate2D sphereSurfaceCoordinate);
    }
}