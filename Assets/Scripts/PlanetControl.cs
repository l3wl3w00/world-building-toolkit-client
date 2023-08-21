using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using WorldBuilder.Client.Line;
using WorldBuilder.Client.Zoom;

namespace WorldBuilder.Client
{
    public class PlanetControl : MonoBehaviour
    {
        private const float Speed = 10.0f;
        private const float LineWidthScale = 0.02f;
        private static readonly Vector3 PlanetUp = new Vector3(0,1,0);
        private Vector3 _lastMousePosition;
        private LineAlongSphere _line;
        private LineRenderer _lineRenderer;
        
        
        public event Action<Transform> PlanetRotatedEvent;
        
        [SerializeField]
        public GameObject lineControlPrefab;
        [SerializeField]
        public Camera mainCamera;

    
        public float Radius { get; set; }

        private void Start()
        {
            _line = new LineAlongSphere(GetComponent<LineRenderer>(), this);
            _lineRenderer = GetComponent<LineRenderer>();
            if (mainCamera == null)
            {
                Debug.Log("Main camera was not set manually");
                mainCamera = Camera.main;
            }
        }

        private void Update()
        {
            var cameraPlanetEdgeDistance = transform.EdgeDistanceFromCamera(mainCamera);

            _lineRenderer.endWidth = cameraPlanetEdgeDistance * LineWidthScale;
            _lineRenderer.startWidth = cameraPlanetEdgeDistance * LineWidthScale;
            if (Input.GetMouseButtonDown(0))
            {
                // When the mouse is clicked, record the position
                _lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                // When the mouse is held down, rotate the sphere
                var delta = Input.mousePosition - _lastMousePosition;
                _lastMousePosition = Input.mousePosition;

                var axis = new Vector3(-delta.y, delta.x, 0);
                
                var selfTransform = transform;
                selfTransform.Rotate(axis * (Speed * Time.deltaTime), Space.World);
                PlanetRotatedEvent?.Invoke(selfTransform);
            }
        }

        private void OnMouseDown()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var raycastSuccessful = Physics.Raycast(ray, out var hit);

            if (!raycastSuccessful)  return;

            var hitCollider = hit.collider as SphereCollider;

            if (hitCollider == null) 
            {
                Debug.Log("ray hit something that is not a SphereCollider");
                return;
            }
            _line.ClickedOnSphere(hit.point);
            var position = transform.position;
            var angle = Vector3.Angle(PlanetUp, hit.point - position);
            var axis = Vector3.Cross(PlanetUp, hit.point - position);
            var controlPoint = Instantiate(lineControlPrefab, hit.point, Quaternion.AngleAxis(angle,axis));
            controlPoint.GetComponent<LineControlPointControl>().Camera = mainCamera;
            controlPoint.transform.localScale *= 0.03f;
            controlPoint.transform.parent = transform;
        }
    }
}