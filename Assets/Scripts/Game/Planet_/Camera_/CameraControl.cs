#nullable enable
using Common.Utils;
using UnityEngine;

namespace Game.Planet_.Camera_
{
    public class CameraControl : MonoBehaviour
    {
        private const float ZoomSpeedMultiplier = 2000f;

        #region Serialized Fields

        public Transform planetTransform = null!; // asserted in Awake

        #endregion

        #region Event Functions

        private void Awake()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), planetTransform);
        }

        private void Update()
        {
            var selfTransform = transform;
            var selfPosition = selfTransform.position;

            var zoomSpeed = Mathf.Sqrt(planetTransform.EdgeDistanceFrom(selfTransform) * ZoomSpeedMultiplier);

            var scrollValue = Input.GetAxis("Mouse ScrollWheel");

            selfPosition += new Vector3(0, 0, scrollValue * Time.deltaTime) * zoomSpeed;
            transform.position = selfPosition;
        }

        #endregion
    }
}