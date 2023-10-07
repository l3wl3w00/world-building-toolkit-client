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
        private Option<ContinentControlPointHandler> _continentControlPointHandler = Option<ContinentControlPointHandler>.None;
        private Option<ContinentLineRendererHandler> _continentLineHandler = Option<ContinentLineRendererHandler>.None;
        private Option<ContinentMeshHandler> _continentMeshHandler = Option<ContinentMeshHandler>.None;
        private Option<PlanetControl> _planetControl = Option<PlanetControl>.None;

        private ContinentLineRendererHandler ContinentLineRendererHandler => 
            this.LazyInitialize(ref _continentLineHandler);
        private ContinentMeshHandler ContinentMeshHandler => 
            this.LazyInitialize(ref _continentMeshHandler);
        private ContinentControlPointHandler ContinentControlPointHandler =>
            this.LazyInitialize(ref _continentControlPointHandler);

        public List<SphereSurfaceCoordinate> ControlPoints => ContinentControlPointHandler.ControlPoints;
        public PlanetControl PlanetControl => _planetControl.ExpectNotNull(nameof(_planetControl), new Func<PlanetControl>(() => PlanetControl));
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
                if (PlanetControl.ContinentInCreation.NullableValue == this) return ContinentState.InCreation;
                if (PlanetControl.SelectedContinent.NullableValue == this) return ContinentState.Selected;
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
                    PlanetControl.SelectedContinent = Option<ContinentHandler>.None;
                    break;
                case ContinentState.Inactive:
                    PlanetControl.SelectedContinent = Option<ContinentHandler>.Some(this);
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

        private void OnActiveContinentChanged(Option<ContinentHandler> c)
        {
            DrawOrDeleteLines(c);
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

        private void Initialize(
            Option<ContinentDto> continentDto, 
            Camera initialCamera,
            List<SphereSurfaceCoordinate> controlPoints,
            PlanetControl planetControl)
        {
            _planetControl = planetControl;
            ContinentControlPointHandler.Initialize(controlPoints, planetControl);
            ContinentLineRendererHandler.Initialize(initialCamera);
            planetControl.Rotated.AddListener(OnRotated);
            planetControl.ContinentInCreationChanged.AddListener(OnActiveContinentChanged);
            planetControl.SelectedContinentChanged.AddListener(OnSelectedContinentChanged);
            planetControl.Clicked.AddListener(OnPlanetClicked);

            continentDto.DoIfNotNull(dto =>
            {
                ContinentName = dto.Name;
                ContinentDescription = dto.Description;
                Invert = dto.Inverted;
                Id = dto.Id;
            });

        }

        public static ContinentHandler CreateGameObject(PlanetControl planetControl)
        {
            return CreateGameObject(planetControl, Option<ContinentDto>.None, new());
        }

        public static ContinentHandler CreateGameObject(
            PlanetControl planetControl, 
            Option<ContinentDto> continentDto,
            List<SphereSurfaceCoordinate> controlPoints)
        {
            var continent = Prefab.Continent.Instantiate(planetControl.transform).GetComponent<ContinentHandler>();
            continent.Initialize(continentDto, planetControl.MainCamera, controlPoints, planetControl);
            continent.name = "Continent" + _continentCounter++;
            continent.UpdateMesh();
            return continent;
        }
    }
}