#nullable enable
using System;
using System.Collections.Generic;
using Common;
using Game.Client.Dto;
using Game.Geometry.Sphere;
using Game.Planet;
using Game.Util;
using Generated;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Continent
{
    public enum ContinentState
    {
        InCreation,
        Selected,
        Inactive
    }

    public class ContinentHandler : MonoBehaviour
    {
        private static long _continentCounter;
        private Camera? _camera;
        private ContinentControlPointHandler? _continentControlPointHandler;

        private ContinentLineRendererHandler? _continentLineHandler;
        private ContinentMeshHandler? _continentMeshHandler;
        private PlanetControl? _planetControl;

        private ContinentLineRendererHandler ContinentLineRendererHandler =>
            this.LazyInitialize(ref _continentLineHandler);

        private ContinentMeshHandler ContinentMeshHandler => this.LazyInitialize(ref _continentMeshHandler);

        private ContinentControlPointHandler ContinentControlPointHandler =>
            this.LazyInitialize(ref _continentControlPointHandler);

        public List<SphereSurfaceCoordinate> ControlPoints => ContinentControlPointHandler.ControlPoints;
        public PlanetControl PlanetControl => ContinentControlPointHandler.PlanetControl;
        public UnityEvent Selected { get; } = new();
        public UnityEvent DeSelected { get; } = new();

        public bool Invert
        {
            get => ContinentMeshHandler.Invert;
            set => ContinentMeshHandler.Invert = value;
        }

        public string ContinentName { get; set; } = "";
        public string ContinentDescription { get; set; } = "";

        public ContinentState ContinentState
        {
            get
            {
                if (_planetControl.ContinentInCreation == this) return ContinentState.InCreation;
                if (_planetControl.SelectedContinent.NullableValue == this) return ContinentState.Selected;
                return ContinentState.Inactive;
            }
        }

        public Guid Id { get; set; }

        #region Event Functions

        private void OnMouseDown()
        {
            switch (ContinentState)
            {
                case ContinentState.Selected:
                    _planetControl.SelectedContinent = Option<ContinentHandler>.None;
                    break;
                case ContinentState.Inactive:
                    _planetControl.SelectedContinent = Option<ContinentHandler>.Some(this);
                    break;
                case ContinentState.InCreation: break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        public void UpdateMesh()
        {
            ContinentMeshHandler.UpdateMesh();
        }

        public void ConnectLineEnds()
        {
            ContinentControlPointHandler.ConnectLineEnds();
        }

        /// <summary>
        ///     Makes the boundary lines of the continent visible if and only if the parameter is the same as this
        /// </summary>
        private void DrawOrDeleteLines(Option<ContinentHandler> c)
        {
            if (c.NullableValue != this)
            {
                ContinentLineRendererHandler.DeleteLines();
                return;
            }

            ContinentControlPointHandler.ReCreateGlobalPoints();
            ContinentLineRendererHandler.UpdateLineRenderer();
        }

        private void OnActiveContinentChanged(ContinentHandler? c)
        {
            DrawOrDeleteLines(c.ToOption());
        }

        private void OnSelectedContinentChanged(Option<ContinentHandler> oldContinent,
            Option<ContinentHandler> newContinent)
        {
            if (oldContinent.HasValue && newContinent.NoValue) DeSelected.Invoke();
            if (oldContinent.NoValue && newContinent.HasValue) Selected.Invoke();

            DrawOrDeleteLines(newContinent);
        }

        private void OnPlanetClicked(Vector3 intersection)
        {
            if (ContinentState is not ContinentState.InCreation) return;

            var leftClick = Input.GetMouseButtonDown(0);
            if (!leftClick) return;

            ContinentControlPointHandler.AddIntersection(intersection);
            ContinentControlPointHandler.ReCreateGlobalPoints();
            ContinentLineRendererHandler.UpdateLineRenderer();
        }

        private void OnMouseDownContinentSelected()
        {
        }

        private void OnRotated()
        {
            if (ContinentState is ContinentState.Selected or ContinentState.InCreation)
            {
                ContinentControlPointHandler.ReCreateGlobalPoints();
                ContinentLineRendererHandler.UpdateLineRenderer();
                return;
            }

            ContinentLineRendererHandler.DeleteLines();
        }

        private void Initialize(ContinentDto? continentDto, Camera initialCamera,
            List<SphereSurfaceCoordinate> controlPoints,
            PlanetControl planetControl)
        {
            _camera = initialCamera;
            _planetControl = planetControl;
            ContinentControlPointHandler.Initialize(controlPoints, planetControl);
            ContinentLineRendererHandler.Initialize(initialCamera);
            planetControl.Rotated.AddListener(OnRotated);
            planetControl.ContinentInCreationChanged.AddListener(OnActiveContinentChanged);
            planetControl.SelectedContinentChanged.AddListener(OnSelectedContinentChanged);
            planetControl.Clicked.AddListener(OnPlanetClicked);

            if (continentDto == null) return;

            ContinentName = continentDto.name;
            ContinentDescription = continentDto.description;
            Invert = continentDto.inverted;

            TrySetId();
            return;

            void TrySetId()
            {
                try
                {
                    Id = Guid.Parse(continentDto.id);
                }
                catch (FormatException)
                {
                    Debug.LogError($"Id form ContinentDto is not in proper format: {continentDto}");
                    Id = Guid.Empty;
                }
            }
        }


        public static ContinentHandler CreateGameObject(PlanetControl planetControl, ContinentDto? continentDto = null,
            List<SphereSurfaceCoordinate>? controlPoints = null, Camera? camera = null)
        {
            camera ??= Camera.main!;
            controlPoints ??= new List<SphereSurfaceCoordinate>();

            var continent = Prefab.Continent.Instantiate(planetControl.transform).GetComponent<ContinentHandler>();
            continent.Initialize(continentDto, camera, controlPoints, planetControl);
            continent.name = "Continent" + _continentCounter++;
            continent.UpdateMesh();
            return continent;
        }
    }
}