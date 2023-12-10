#nullable enable
using Common.Utils;
using Game.Planet_.Parts;
using UnityEngine;
using Zenject;

namespace Game.Planet_.Camera_
{
    public class CameraControl : MonoBehaviour
    {
        private const float ZoomSpeedMultiplier = 2000f;

        #region Serialized Fields

        [Inject] private PlanetMonoBehaviour _planetMono = null!; // asserted in Awake

        #endregion

        #region Event Functions

        private void Awake()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _planetMono);
        }

        private void Update()
        {
            if (!_planetMono.ReactToUserInput) return;
            var selfTransform = transform;
            var selfPosition = selfTransform.position;

            var zoomSpeed = Mathf.Sqrt(_planetMono.transform.EdgeDistanceFrom(selfTransform) * ZoomSpeedMultiplier);

            var scrollValue = Input.GetAxis("Mouse ScrollWheel");

            selfPosition += new Vector3(0, 0, scrollValue * Time.deltaTime) * zoomSpeed;
            transform.position = selfPosition;
        }

        #endregion
    }
}