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

        [SerializeField] private float rotationSpeed = 0.5f;
        
        #region Member Variables

        private Vector3 _lastMousePosition;

        #endregion

        public UnityEvent Rotated { get; } = new();
        public UnityEvent<Option<ContinentHandler>> ContinentInCreationChanged { get; } = new();

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
        
        void CreateContinents(WorldDetailedDto worldDetailed)
        {
            foreach (var continent in worldDetailed.Continents)
            {
                var controlPoints = continent.Bounds
                    .Select(b =>
                        new SphereSurfaceCoordinate(b.Radius, b.Polar.AsRadians(), b.Azimuthal.AsRadians()))
                    .ToList();
                ContinentHandler.CreateGameObject(this, continent, controlPoints);
            }
        }
        #endregion

        public void DestroyActiveContinent()
        {
            ContinentInCreation.DoIfNotNull(c => Destroy(c.gameObject));
        }

        #region EditorFields

        [SerializeField] private GameObject continentPrefab = null!; // asserted in Awake
        [SerializeField] private Camera mainCamera = null!; // asserted in Awake
        private Option<ContinentHandler> _continentInCreation = Option<ContinentHandler>.None;
        private Option<ContinentHandler> _selectedContinent = Option<ContinentHandler>.None;

        #endregion

        #region Properties
        
        public Option<ContinentHandler> ContinentInCreation
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
                if (ContinentInCreation.HasValue && value.HasValue)
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

        public Model.Planet Planet { get; set; }

        public ICoordinate3D Center => transform.position.ToCartesian();

        public Quaternion Rotation => transform.rotation;

        public float Radius => transform.localScale.x;
        public Camera MainCamera => mainCamera;

        #endregion

        #region EventFunctions

        #region Awake

        private void Awake()
        {
            NullChecker.AssertNoneIsNullInType(GetType(),mainCamera, continentPrefab);

            var sceneChangeParameters = ISceneChangeParameters.Instance;
            var world = GetWorld();

            Planet = DtoToPlanet(world);
            CreateContinents(world);
            
            return;
            WorldDetailedDto GetWorld()
            {
                return sceneChangeParameters.GetNonNullable<WorldDetailedDto>(SceneParamKey.WorldDetailed);
            }
            
            Model.Planet DtoToPlanet(Option<WorldDetailedDto> worldDetailedDto)
            {
                return worldDetailedDto.Map(
                    onHasValue: w => new Model.Planet(w.Id, w.Name, w.Description),
                    valueOnNull: new Model.Planet(Guid.Empty));
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
                var cameraPlanetEdgeDistance = selfTransform.EdgeDistanceFromCamera(mainCamera);

                // When the mouse is held down, rotate the sphere
                var delta = _lastMousePosition - Input.mousePosition;
                _lastMousePosition = Input.mousePosition;

                var axis = new Vector3(-delta.y, delta.x, 0);

                selfTransform.Rotate(axis * (rotationSpeed * Time.deltaTime * cameraPlanetEdgeDistance), Space.World);

                Rotated.Invoke();
            }
        }

        #endregion

        private void OnMouseDown()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var raycastSuccessful = Physics.Raycast(ray, out var hit);

            if (!raycastSuccessful) return;

            hit.collider.Cast<Collider, SphereCollider>()
                .ExpectNotNull("ray hit something that is not a SphereCollider");

            Clicked.Invoke(hit.point);
        }

        #endregion
    }
}