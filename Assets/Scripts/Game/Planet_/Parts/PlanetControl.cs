#nullable enable
using Common;
using Common.Model;
using Common.SceneChange;
using Common.Utils;
using Game.Planet_.Camera_;
using ProceduralToolkit;
using UnityEngine;

namespace Game.Planet_.Parts
{
    internal class PlanetControl : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 0.5f;
        [SerializeField] private ContinentsManager continentsManager = null!; // asserted in Awake
        [SerializeField] private GameObject continentPrefab = null!; // asserted in Awake
        [SerializeField] private Camera mainCamera = null!; // asserted in Awake
        private Vector3 _lastMousePosition;
        
        internal Planet Planet { get; set; }

        internal Camera MainCamera => mainCamera;
        

        private void Awake()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), mainCamera, continentPrefab, continentsManager);

            SceneChangeParameters
                .SearchInSceneAndExpectFound(GetType())
                .GetOrLogError(SceneParamKeys.WorldInitializeParams)
                .DoIfNotNull(i => Planet = i.Planet);
        }


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
                continentsManager.UpdatePlanetLines();
            }
        }
        
        internal MeshDraft CreateSphereMeshDraft(int horizontalSegments, int verticalSegments)
        {
            var result = MeshDraft.Sphere(1, horizontalSegments, verticalSegments); //TODO

            var trianglesMultipleOf3 = result.triangles.Count % 3 == 0;
            if (!trianglesMultipleOf3) Debug.LogError("triangle count in MeshDraft is not divisible by 3!");

            return result;
        }
    }
}