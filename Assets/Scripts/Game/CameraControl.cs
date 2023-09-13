using UnityEngine;
using WorldBuilder.Client.Game.Zoom;

namespace WorldBuilder.Client.Game
{
    public class CameraControl : MonoBehaviour
    {
        private const float ZoomSpeedMultiplier = 50f;
        public Transform PlanetTransform;

        private void Update()
        {
            var selfTransform = transform;
            var selfPosition = selfTransform.position;
        
            var zoomSpeed = Mathf.Sqrt(PlanetTransform.EdgeDistanceFrom(selfTransform) * ZoomSpeedMultiplier);

            var scrollValue = Input.GetAxis("Mouse ScrollWheel");

            selfPosition += new Vector3(0, 0, scrollValue * Time.deltaTime) * zoomSpeed;
            transform.position = selfPosition;
        }
    }
}
