using UnityEngine;
using WorldBuilder.Client.Zoom;

namespace WorldBuilder.Client.Line
{
    public class LineAlongSphere 
    {
        private readonly DynamicLineRenderer _dynamicLineRenderer;
        private readonly Transform _planetTransform;
        private Vector3 _previousIntersection;
        private const int Resolution = 100;
        private const float LineOffsetScale = 1.005f;
        private int _clickCount = 0;

        public LineAlongSphere(LineRenderer lineRenderer, PlanetControl planetControl)
        {
            _dynamicLineRenderer = new DynamicLineRenderer(lineRenderer, planetControl);
            _planetTransform = planetControl.transform;
        }
    
        public void ClickedOnSphere(Vector3 intersection)
        {
            _clickCount++;
            if (_clickCount == 1) 
            {
                _dynamicLineRenderer.SetNextLinePosition(intersection);
                _previousIntersection = intersection;
                return;
            }
            
            DrawArch(_previousIntersection, intersection);
            _previousIntersection = intersection;
        }
    
        private void DrawArch(Vector3 start, Vector3 end)
        {
            var center = _planetTransform.position;
            var centerToStart = (start - center) * LineOffsetScale;
            var centerToEnd = (end - center) * LineOffsetScale;
    
            var axis = Vector3.Cross(centerToStart, centerToEnd).normalized;
            var angle = Vector3.Angle(centerToStart, centerToEnd);
            
            if (Mathf.Approximately(angle, 0))
            {
                return;
            }

            var resolution = GetResolution(start, end);

            var step = angle / resolution;
            for (float currentAngle = 0; currentAngle < angle + 0.000001f; currentAngle += step)
            {
                var pointOnArc = Quaternion.AngleAxis(currentAngle, axis) * centerToStart + center;
                _dynamicLineRenderer.SetNextLinePosition(pointOnArc);
            }
        }

        private float GetResolution(Vector3 start, Vector3 end)
        {
            var startEndDistance = (start - end).magnitude;
            var resolution = Resolution;
            if (startEndDistance < 1)
            {
                resolution = Resolution / 10;
            }
            if (startEndDistance < 0.1)
            {
                resolution = 1;
            }

            return resolution;
        }
    }
}
