using LineUtils;
using UnityEngine;

namespace LineUtils
{
    public class LineAlongSphere 
    {
        private readonly DynamicLineRenderer _dynamicLineRenderer;
        private readonly Transform _gameObjectTransform;
        private Vector3 _previousIntersection;
        private const int Resolution = 100;
        private int _clickCount = 0;
    
        public LineAlongSphere(LineRenderer lineRenderer, Transform gameObjectTransform)
        {
            _dynamicLineRenderer = new DynamicLineRenderer(lineRenderer);
            _gameObjectTransform = gameObjectTransform;
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
            var center = _gameObjectTransform.position;
            var centerToStart = start - center;
            var centerToEnd = end - center;
    
            var axis = Vector3.Cross(centerToStart, centerToEnd).normalized;
            var angle = Vector3.Angle(centerToStart, centerToEnd);
            if (Mathf.Approximately(angle, 0))
            {
                return;
            }
    
            var step = angle / Resolution;
            for (float currentAngle = 0; currentAngle < angle + 0.000001f; currentAngle += step)
            {
                var pointOnArc = (Quaternion.AngleAxis(currentAngle, axis) * (centerToStart)) + center;
                _dynamicLineRenderer.SetNextLinePosition(pointOnArc);
            }
        }
    }
}
