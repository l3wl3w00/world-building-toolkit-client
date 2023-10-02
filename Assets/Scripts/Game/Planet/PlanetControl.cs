#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Game.Camera_;
using Game.Client.Dto;
using Game.Continent;
using Game.Geometry.Coordinate._3D;
using Game.Geometry.Sphere;
using Game.SceneChange;
using Game.Util;
using ProceduralToolkit;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Planet
{
    public class PlanetControl : MonoBehaviour, ISphere
    {
        #region Constants

        private const float Speed = 0.5f;

        #endregion

        #region Member Variables

        private Vector3 _lastMousePosition;

        #endregion

        public UnityEvent Rotated { get; } = new();
        public UnityEvent<ContinentHandler?> ContinentInCreationChanged { get; } = new();

        public UnityEvent<Option<ContinentHandler>, Option<ContinentHandler>> SelectedContinentChanged { get; } = new();

        public UnityEvent<Vector3> Clicked { get; } = new();

        #region ISphere Members

        public IEnumerable<(int, int, int)> Triangles(int verticalResolution, int horizontalResolution, float height,
            List<Vector3> vertices)
        {
            var sphereMeshDraft = MeshDraft.Sphere(Radius, horizontalResolution, verticalResolution);
            var triangles = sphereMeshDraft.triangles;
            vertices.AddRange(sphereMeshDraft.vertices);
            if (triangles.Count % 3 != 0) Debug.LogError("triangle count in MeshDraft is not divisible by 3!");
            for (var i = 0; i < triangles.Count / 3; i++)
            {
                var v1Index = triangles[i * 3 + 0];
                var v2Index = triangles[i * 3 + 1];
                var v3Index = triangles[i * 3 + 2];

                yield return (v1Index, v2Index, v3Index);
            }
        }

        #endregion


        #region LoadContinents

        private void LoadContinents(WorldDetailedDto? worldDetailed)
        {
            if (worldDetailed == null) return;
            foreach (var continent in worldDetailed.continents)
            {
                var controlPoints = continent.bounds
                    .Select(b =>
                        new SphereSurfaceCoordinate(b.radius, b.polar.AsRadians(), b.azimuthal.AsRadians()))
                    .ToList();
                CreateContinent(continent, controlPoints);
            }

            return;

            void CreateContinent(ContinentDto continentDto, List<SphereSurfaceCoordinate>? controlPoints)
            {
                ContinentHandler.CreateGameObject(this, continentDto, controlPoints, mainCamera);
            }
        }

        #endregion

        public void DestroyActiveContinent()
        {
            if (ContinentInCreation == null) return;
            Destroy(ContinentInCreation!.gameObject);
        }

        #region EditorFields

        [SerializeField] private GameObject? continentPrefab;
        [SerializeField] private Camera? mainCamera;
        private ContinentHandler? _continentInCreation;
        private Option<ContinentHandler> _selectedContinent = Option<ContinentHandler>.None;

        #endregion

        #region Properties

        public Camera? MainCamera => mainCamera;

        public ContinentHandler? ContinentInCreation
        {
            get => _continentInCreation;
            set
            {
                _continentInCreation = value;
                ContinentInCreationChanged.Invoke(value);
                SelectedContinent = Option<ContinentHandler>.None;
            }
        }

        public Option<ContinentHandler> SelectedContinent
        {
            get => _selectedContinent;
            set
            {
                if (ContinentInCreation != null && value.HasValue)
                {
                    Debug.LogWarning("tried to select continent while another is in creation");
                    return;
                }

                var previous = _selectedContinent;
                _selectedContinent = value;
                SelectedContinentChanged.Invoke(previous, value);
            }
        }

        public bool IsAnyContinentSelected => !NoContinentSelected;
        public bool NoContinentSelected => SelectedContinent.NoValue;

        public Planet Planet { get; set; }

        public ICoordinate3D Center => transform.position.ToCartesian();

        public Quaternion Rotation => transform.rotation;

        public float Radius => transform.localScale.x;

        #endregion

        #region EventFunctions

        #region Awake

        private void Awake()
        {
            SetCameraIfNull();

            var sceneChangeParameters = ISceneChangeParameters.Instance;
            var world = GetWorld();

            Planet = DtoToPlanet(world);
            LoadContinents(world);
            return;

            WorldDetailedDto? GetWorld()
            {
                return sceneChangeParameters.Get<WorldDetailedDto>(SceneParamKey.WorldDetailed);
            }

            void SetCameraIfNull()
            {
                if (MainCamera != null) return;
                Debug.Log("Main camera was not set manually");
                mainCamera = Camera.main;
            }

            Planet DtoToPlanet(WorldDetailedDto? worldDetailedDto)
            {
                if (worldDetailedDto == null) return new Planet(Guid.Empty);

                return new Planet(world.id, world.name, world.description);
            }
        }

        #endregion

        #region Update

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                // When the mouse is clicked, record the position
                _lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(1))
            {
                var selfTransform = transform;
                var cameraPlanetEdgeDistance = selfTransform.EdgeDistanceFromCamera(MainCamera);

                // When the mouse is held down, rotate the sphere
                var delta = _lastMousePosition - Input.mousePosition;
                _lastMousePosition = Input.mousePosition;

                var axis = new Vector3(-delta.y, delta.x, 0);

                selfTransform.Rotate(axis * (Speed * Time.deltaTime * cameraPlanetEdgeDistance), Space.World);

                Rotated.Invoke();
            }
        }

        #endregion

        private void OnMouseDown()
        {
            var ray = mainCamera!.ScreenPointToRay(Input.mousePosition);
            var raycastSuccessful = Physics.Raycast(ray, out var hit);

            if (!raycastSuccessful) return;

            var hitCollider = hit.collider as SphereCollider;

            if (hitCollider == null)
            {
                Debug.Log("ray hit something that is not a SphereCollider");
                return;
            }

            Clicked.Invoke(hit.point);
        }

        #endregion
    }
}