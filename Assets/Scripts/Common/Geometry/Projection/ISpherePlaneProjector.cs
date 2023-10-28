#nullable enable
using Common.Geometry.Coordinate._2D;
using Common.Geometry.Sphere;
using Common.Model;
using UnityEngine;

namespace Common.Geometry.Projection
{
    public interface ISpherePlaneProjector
    {
        CartesianCoordinate2D SphereToPlane(SphereSurfaceCoordinate sphereSurfaceCoordinate);
        Vector2 SphereToPlane(Vector3 cartesian);
        unsafe Vector2 SphereToPlaneUnsafe(Vector3* vec);
        SphereSurfaceCoordinate PlaneToSphere(CartesianCoordinate2D sphereSurfaceCoordinate);
    }
}