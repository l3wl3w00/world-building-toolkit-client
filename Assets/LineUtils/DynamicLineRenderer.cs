using UnityEngine;

namespace LineUtils
{
    public class DynamicLineRenderer
    {
        private readonly LineRenderer _lineRenderer;
        private int _positionCount = 0;
    
        public DynamicLineRenderer(LineRenderer lineRenderer)
        {
            _lineRenderer = lineRenderer;
        }
    
        public void SetNextLinePosition(Vector3 position)
        {
            _positionCount++;
            _lineRenderer.positionCount = _positionCount;
            _lineRenderer.SetPosition(_positionCount - 1, position);
        }
    }
}

