using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldBuilder.Client.Game.Common.Coordinate;

namespace WorldBuilder.Client.Game.Common.Sphere
{
    public delegate void ActionOnIndexedCoordinate(Vector3 coordinate);
    public class PointsOnSphereHandler
    {
        private readonly List<ICoordinate> _intersectionCoordinates = new List<ICoordinate>();
        private ISphere _sphere;

        public PointsOnSphereHandler(ISphere sphere)
        {
            _sphere = sphere;
        }

        public ISphere Sphere
        {
            get => _sphere;
            set => _sphere = value;
        }

        public void ForEachGlobalCoordinate(ActionOnIndexedCoordinate action)
        {
            foreach (var c in _intersectionCoordinates)
            {
                var globalCoordinate = ToGlobalCoordinate(c);
                action?.Invoke(globalCoordinate);
            }
        }
        
        public IEnumerable<Vector3> GetGlobalCoordinates()
        {
            return _intersectionCoordinates.Select(ToGlobalCoordinate);
        }
        
        public List<Vector3> GetGlobalCoordinatesList()
        {
            var result = new List<Vector3>();
            foreach (var coordinate in _intersectionCoordinates)
            {
                result.Add(ToGlobalCoordinate(coordinate));
            }
            return result;
        }

        private Vector3 ToGlobalCoordinate(ICoordinate coordinate)
        {
            var rotatedCoordinate = _sphere.Rotation * coordinate.ToCartesianVector3();
            return rotatedCoordinate + _sphere.Center.ToCartesianVector3();
        }

        public void AddIntersection(Vector3 intersection)
        {
            var localIntersection = intersection - _sphere.Center.ToCartesian().ToVector3();
            localIntersection = Quaternion.Inverse(_sphere.Rotation) * localIntersection;
            _intersectionCoordinates.Add(new CartesianCoordinate(localIntersection));
        }

    }
}