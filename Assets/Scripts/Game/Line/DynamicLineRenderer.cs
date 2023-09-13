using System;
using UnityEngine;

namespace WorldBuilder.Client.Game.Line
{
    public class DynamicLineRenderer
    {
        private readonly LineRenderer _lineRenderer;
        private int _positionCount = 0;

        public DynamicLineRenderer(LineRenderer lineRenderer)
        {
            _lineRenderer = lineRenderer;
        }

        private int PositionCount
        {
            get => _positionCount;
            set
            {
                _positionCount = value;
                _lineRenderer.positionCount = value;
            }
        }

        public void SetNextLinePosition(Vector3 position)
        {
            PositionCount++;
            _lineRenderer.SetPosition(PositionCount - 1, position);
        }
        
        public void ForEachPosition(Func<int, Vector3> function)
        {
            for (int i = 0; i < PositionCount; i++)
            {
                _lineRenderer.SetPosition(i, function(i));
            }
        }

        public void ResetPositionCount()
        {
            PositionCount = 0;
        }
        
        public void ResetOptimized()
        {
            _positionCount = 0;
        }
    }
}

