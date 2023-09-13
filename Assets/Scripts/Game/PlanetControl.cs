using UnityEngine;
using WorldBuilder.Client.Game.Common.Constants;
using WorldBuilder.Client.Game.Common.Coordinate;
using WorldBuilder.Client.Game.Common.Sphere;
using WorldBuilder.Client.Game.Edit;
using WorldBuilder.Client.Game.Edit.State;
using WorldBuilder.Client.Game.Line;
using WorldBuilder.Client.Game.Zoom;

namespace WorldBuilder.Client.Game
{
    public class PlanetControl : MonoBehaviour, ISphere
    {
        #region Constants
        private const float Speed = 10.0f;
        private const float LineWidthScale = 0.02f;
        #endregion
 
        #region Member Variables
        private Vector3 _lastMousePosition;
        private Planet _planet;
        private IEditPlanetState _editPlanetState;
        #endregion

        #region EditorFields
        [SerializeField] private GameObject lineControlPrefab;
        [SerializeField] private GameObject lineRendererPrefab;
        [SerializeField] private Camera mainCamera;
        #endregion

        #region Properties
        public ICoordinate Center => transform.position.ToCartesian();
        public Quaternion Rotation => transform.rotation;
        public Camera MainCamera => mainCamera;
        public IEditPlanetState EditPlanetState
        {
            get => _editPlanetState;
            set => _editPlanetState = value;
        }
        public float Radius { get; } //TODO assign it somewhere
        public Planet Planet => _planet;
        #endregion

        private void Start()
        {
            _planet = FindObjectOfType<PlanetStateHolder>().Planet;
            _planet.RendererFactory =
                () => Instantiate(lineRendererPrefab, transform).GetComponent<LineRenderer>();
            _editPlanetState = new DefaultEditPlanetState(_planet);
            if (MainCamera == null)
            {
                Debug.Log("Main camera was not set manually");
                mainCamera = Camera.main;
            }
        }

        private void Update()
        {
            UpdateLineWidth();
            HandleRotation();
        }

        private void HandleRotation()
        {
            if (Input.GetMouseButtonDown(1))
            {
                // When the mouse is clicked, record the position
                _lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(1))
            {
                // When the mouse is held down, rotate the sphere
                var delta = _lastMousePosition - Input.mousePosition;
                _lastMousePosition = Input.mousePosition;

                var axis = new Vector3(-delta.y, delta.x, 0);

                var selfTransform = transform;
                selfTransform.Rotate(axis * (Speed * Time.deltaTime), Space.World);
                _editPlanetState.OnRotate(selfTransform);
            }
        }

        private void UpdateLineWidth()
        {
            var cameraPlanetEdgeDistance = transform.EdgeDistanceFromCamera(MainCamera);

            foreach (var lineRenderer in _planet.LineRenderers)
            {
                lineRenderer.endWidth = cameraPlanetEdgeDistance * LineWidthScale;
                lineRenderer.startWidth = cameraPlanetEdgeDistance * LineWidthScale;
            }
        }

        private void OnMouseDown()
        {
            _editPlanetState.OnClick(transform);
        }
    }
}