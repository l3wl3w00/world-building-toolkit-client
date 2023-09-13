using System.Collections.Generic;
using UnityEngine;
using WorldBuilder.Client.Game.Common.Coordinate;

namespace WorldBuilder.Client.Game.Common.Sphere
{
    public class Arch
    {
        private const float ArchOffsetScale = 1.005f;
        
        private readonly ISphere _sphere;
        private readonly ICoordinate _start;
        private readonly ICoordinate _end;

        public Arch(ISphere sphere, ICoordinate start, ICoordinate end)
        {
            _sphere = sphere;
            _start = start;
            _end = end;
        }

        public IEnumerable<Vector3> GetGlobalPoints(int resolution = 100)
        {
            var centerGlobal = _sphere.Center.ToCartesianVector3();
            var startGlobal = _start.ToCartesianVector3();
            var endGlobal = _end.ToCartesianVector3();
            
            var centerToStart = (startGlobal - centerGlobal) * ArchOffsetScale;
            var centerToEnd = (endGlobal - centerGlobal) * ArchOffsetScale;
    
            var axis = Vector3.Cross(centerToStart, centerToEnd).normalized;
            var angle = Vector3.Angle(centerToStart, centerToEnd);
            
            if (Mathf.Approximately(angle, 0))
            {
                yield break;
            }

            resolution = GetResolution(resolution);

            var step = angle / resolution;
            for (float currentAngle = 0; currentAngle < angle + 0.000001f; currentAngle += step)
            {
                yield return Quaternion.AngleAxis(currentAngle, axis) * centerToStart + centerGlobal;
            }
        }
        
        private int GetResolution(int resolution)
        {            
            var startGlobal = _start.ToCartesianVector3();
            var endGlobal = _end.ToCartesianVector3();
            
            var startEndDistance = (startGlobal - endGlobal).magnitude;
            if (startEndDistance < 1)
            {
                resolution = resolution / 10;
            }
            if (startEndDistance < 0.1)
            {
                resolution = 1;
            }

            return resolution;
        }

    }
}