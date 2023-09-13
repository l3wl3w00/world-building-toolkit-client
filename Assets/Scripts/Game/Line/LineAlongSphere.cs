using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using WorldBuilder.Client.Game.Common.Coordinate;
using WorldBuilder.Client.Game.Common.Linq;
using WorldBuilder.Client.Game.Common.Sphere;

namespace WorldBuilder.Client.Game.Line
{
    public class LineAlongSphere 
    {
        private readonly ISphere _sphere;
        private readonly List<Vector3> _globalPoints = new();
        private readonly PointsOnSphereHandler _pointsOnSphereHandler;

        public LineAlongSphere(ISphere sphere)
        {
            _sphere = sphere;
            _pointsOnSphereHandler = new PointsOnSphereHandler(sphere);
        }

        public List<Vector3> GlobalPoints => _globalPoints;

        public void ClickedOnSphere(Vector3 intersection)
        {
            _pointsOnSphereHandler.AddIntersection(intersection);
            Update();
        }
        

        public void Update()
        {
            _globalPoints.Clear();
            
            var coordinates = _pointsOnSphereHandler.GetGlobalCoordinates().ToList();
            if (coordinates.Count < 1) return;

            var previous = coordinates[0];
            coordinates
                .Skip(1)
                .Select(coordinate =>
                {
                    var result = new Arch(_sphere, previous.ToCartesian(), coordinate.ToCartesian());
                    previous = coordinate;
                    return result;
                })
                .Select(arch => arch.GetGlobalPoints())
                .ForEach(points =>
                {
                    _globalPoints.AddRange(points);
                });
        }

        public void Render(LineRenderer lineRenderer)
        {
            lineRenderer.positionCount = _globalPoints.Count;

            for (int i = 0; i < _globalPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, _globalPoints[i]);
            }
        }
    }
}
