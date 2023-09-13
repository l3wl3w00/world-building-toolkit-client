using UnityEngine;
using WorldBuilder.Client.Game.Common.Constants;
using WorldBuilder.Client.Game.Common.Sphere;
using WorldBuilder.Client.Game.Line;

namespace WorldBuilder.Client.Game.Edit.State
{
    public delegate GameObject ControlPointFactory(Vector3 position, Quaternion rotation);
    
    public class BoundedLocationEditingState : IEditPlanetState
    {
        private ControlPointFactory controlPointFactory;
        private readonly Planet _planet;
        private LineAlongSphere _line;
        private Camera _mainCamera;
        private IPlanetConstants _planetConstants;

        public BoundedLocationEditingState(
            ControlPointFactory controlPointFactory,
            Planet planet,
            Camera mainCamera, 
            IPlanetConstants planetConstants, 
            ISphere sphere)
        {
            this.controlPointFactory = controlPointFactory;
            _planet = planet;
            _line = new LineAlongSphere(sphere);
            _planet.AddLine(_line);
            _mainCamera = mainCamera;
            _planetConstants = planetConstants;
        }

        public void OnClick(Transform selfTransform)
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var raycastSuccessful = Physics.Raycast(ray, out var hit);

            if (!raycastSuccessful)  return;

            var hitCollider = hit.collider as SphereCollider;

            if (hitCollider == null) 
            {
                Debug.Log("ray hit something that is not a SphereCollider");
                return;
            }
            _line.ClickedOnSphere(hit.point);
            var controlPoint = CreateControlPoint(selfTransform, hit);
            _planet.RenderUpdatedLines();
        }

        public void OnRotate(Transform selfTransform)
        {
            _planet.RenderUpdatedLines();
        }

        public void OnCancel()
        {
            _planet.DeleteLine(_line);
            _planet.RenderUpdatedLines();
        }

        private GameObject CreateControlPoint(Transform selfTransform, RaycastHit hit)
        {
            var position = selfTransform.position;
            var angle = Vector3.Angle(_planetConstants.PlanetUp, hit.point - position);
            var axis = Vector3.Cross(_planetConstants.PlanetUp, hit.point - position);
            var controlPoint = controlPointFactory.Invoke(hit.point, Quaternion.AngleAxis(angle, axis));
            controlPoint.GetComponent<LineControlPointControl>().Camera = _mainCamera;
            controlPoint.transform.localScale *= 0.03f;
            controlPoint.transform.parent = selfTransform;
            return controlPoint;
        }
    }
}