using UnityEngine;

namespace WorldBuilder.Client.Game
{
    public class MousePositionHandler
    {
        private Vector3 _mousePosition;

        public MousePositionHandler(Vector3 initialMousePosition)
        {
            Update(initialMousePosition);
        }

        public Vector3 MouseDelta { get; private set; }

        public void Update(Vector3 currentMousePosition)
        {
            _mousePosition = currentMousePosition;
            MouseDelta = currentMousePosition - _mousePosition;
        }
    }
}