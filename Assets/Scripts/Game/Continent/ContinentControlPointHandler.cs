using System.Collections.Generic;
using System.Linq;
using Game.Geometry.Coordinate._3D;
using Game.Geometry.Sphere;
using Game.Planet;
using UnityEngine;

namespace Game.Continent
{
    internal class ContinentControlPointHandler : MonoBehaviour
    {
        internal PlanetControl PlanetControl { get; private set; }
        internal List<SphereSurfaceCoordinate> ControlPoints { get; } = new();
        internal List<Vector3> DetailedGlobalPoints { get; } = new();

        internal void Initialize(List<SphereSurfaceCoordinate> initialControlPoints, PlanetControl planetControl)
        {
            ControlPoints.AddRange(initialControlPoints);
            PlanetControl = planetControl;
        }

        internal void AddIntersection(Vector3 globalIntersection)
        {
            var localIntersection = globalIntersection - PlanetControl.transform.position;
            localIntersection = Quaternion.Inverse(PlanetControl.Rotation) * localIntersection;
            ControlPoints.Add(SphereSurfaceCoordinate.FromLocalVec3(localIntersection, PlanetControl.Radius));
        }

        internal void ReCreateGlobalPoints()
        {
            DetailedGlobalPoints.Clear();

            var globalControlPoints = ControlPoints.Select(ToGlobalCoordinate).ToList();
            if (globalControlPoints.Count < 1) return;

            var previous = globalControlPoints.First();
            foreach (var coordinate in globalControlPoints.Skip(1))
            {
                var archBetweenPreviousAndCurrent =
                    new Arch(PlanetControl, previous.ToCartesian(), coordinate.ToCartesian());
                var newGlobalPoints = archBetweenPreviousAndCurrent.GetGlobalPoints();
                DetailedGlobalPoints.AddRange(newGlobalPoints);
                previous = coordinate;
            }
        }

        private Vector3 ToGlobalCoordinate(SphereSurfaceCoordinate sphereSurfaceCoordinate)
        {
            var rotatedCoordinate =
                PlanetControl.Rotation * sphereSurfaceCoordinate.ToGlobalCartesianVec3(PlanetControl);
            return rotatedCoordinate + PlanetControl.transform.position;
        }

        public void ConnectLineEnds()
        {
            ControlPoints.Add(ControlPoints.First());
            ReCreateGlobalPoints();
        }
    }
}