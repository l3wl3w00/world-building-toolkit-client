using UnityEngine;

namespace WorldBuilder.Client.Line
{
    public class DynamicLineRenderer
    {
        private readonly LineRenderer _lineRenderer;
        private readonly PlanetControl _planetControl;
        private int _positionCount = 0;
    
        public DynamicLineRenderer(LineRenderer lineRenderer, PlanetControl planetControl)
        {
            _lineRenderer = lineRenderer;
            _planetControl = planetControl;
            planetControl.PlanetRotatedEvent += UpdatePositions;
        }

        private void UpdatePositions(Transform newPlanetTransform)
        {
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                var position = _lineRenderer.GetPosition(i);
                //TODO
            }
        }

        public void SetNextLinePosition(Vector3 position)
        {
            _positionCount++;
            _lineRenderer.positionCount = _positionCount;
            _lineRenderer.SetPosition(_positionCount - 1, position);
        }
    }
}

