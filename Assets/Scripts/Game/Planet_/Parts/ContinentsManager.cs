#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Model;
using Common.Utils;
using Game.Common;
using Game.Continent_;
using Game.Planet_.Parts.State;
using Game.Region_;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Game.Planet_.Parts
{
    internal class ContinentsManager : MonoBehaviour
    {
        private Option<ContinentTree> _continentTree = Option<ContinentTree>.None;
        private IContinentState _continentState = new PlanetInCreationState();
        [SerializeField] private PlanetControl planetControl = null!; //Asserted in Awake
        [Inject] private ModelCollection<Calendar> _calendars = null!; //Asserted in Awake
        [Inject] private ModelCollection<HistoricalEvent> _events = null!; //Asserted in Awake
        [Inject] private ModelCollection<Region> _regions = null!; //Asserted in Awake
        [Inject] private SignalBus _signalBus = null!; //Asserted in Awake
        private IEnumerable<ContinentMonoBehaviour> Continents => ContinentTree.Continents;

        private IContinentState ContinentState
        {
            get => _continentState;
            set
            {
                Debug.Log($"Continent State Changed from {_continentState.ToOption()} to {value}");
                var oldState = _continentState;
                _continentState = value;
                ContinentState.OnStart();
                StateChanged.Invoke(oldState, _continentState);
            }
        }
        
        internal ContinentsState State => ContinentState.State;
        internal ContinentTree ContinentTree => _continentTree.ExpectNotNull("ContinentTree was null when expected not to be");
        public UnityEvent<IContinentState, IContinentState> StateChanged { get; } = new();

        private void Awake()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), planetControl, _calendars, _signalBus);
        }
        
        internal void SetContinentState(IContinentState continentState)
        {
            this.ContinentState = continentState;
        }

        internal void ClickedOnContinent(ContinentMonoBehaviour continentMono)
        {
            ContinentState.ClickedOnContinent(continentMono, planetControl.MainCamera);
        }

        public void ClickedOnRegion(RegionMonoBehaviour region)
        {
            ContinentState.ClickedOnRegion(region, planetControl.MainCamera);
        }

        public void SelectRegion(RegionMonoBehaviour region, PlanetMonoBehaviour planetMono)
        {
            ContinentState = new SelectedRegionState(region, planetMono);
        }

        internal void SelectContinent(ContinentMonoBehaviour continent, PlanetMonoBehaviour planetMono)
        {
            ContinentState = new SelectedContinentState(continent, planetMono);
        }

        internal void StartCreatingNewContinent()
        {
            ContinentState.StartCreatingNewContinent();
        }

        internal void MakeAllLinesInvisible()
        {
            MakeAllLinesInvisibleExcept(Option<BoundedLocationMono>.None);
        }

        internal void MakeAllLinesInvisibleExcept(Option<BoundedLocationMono> continent)
        {
            foreach (var mono in FindObjectsOfType<BoundedLocationMono>())
            {
                mono.LinesVisible = false;
            }

            continent.DoIfNotNull(c => c.LinesVisible = true);
        }

        internal TResult ApplyStateOperation<TState, TResult>(IContinentStateOperation<TState, TResult> operation) 
            where TState: IContinentState
        {
            return ContinentState.Interact(operation);
        }

        internal void UpdatePlanetLines()
        {
            if (_continentTree.NoValue) return;
            // foreach (var mono in Continents)
            // {
            //     mono.UpdateLines();
            // }
            foreach (var regionMonoBehaviour in FindObjectsOfType<BoundedLocationMono>())
            {
                regionMonoBehaviour.UpdateLines();
            }
            _continentState.UpdateVisibleLines();
        }

        internal void ToEditPlanetState(PlanetMonoBehaviour planetMono)
        {
            ContinentState = new EditPlanetState(planetMono);
        }

        internal void StartCreatingNewRegion()
        {
            ContinentState.StartCreatingNewRegion();
        }

        internal void ToEditPlanetStateInitially(PlanetWithRelatedEntities worldInitParams, PlanetMonoBehaviour planetMono)
        {
            _continentTree = ContinentTree
                .Create(worldInitParams.Continents, planetMono, planetMono)
                .ToOption();
            
            foreach (var continent in worldInitParams.Continents)
            {
                foreach (var region in continent.Regions)
                {
                    var regionRef = _regions.Add(region);
                    RegionMonoBehaviour.Create(new(planetMono, regionRef, planetMono));
                }
            }

            _calendars.Add(worldInitParams.Calendars);
            _events.Add(worldInitParams.Events);
            UpdatePlanetMeshes();
            _continentState = new EditPlanetState(planetMono);
            _continentState.OnStart();
            _signalBus.Fire<StateChangedSignal>();
        }

        internal void UpdatePlanetMeshes()
        {
            UpdatePlanetLines();
            var continentTree = _continentTree.ExpectNotNull(nameof(_continentTree), (Action) UpdatePlanetMeshes);
            continentTree.UpdatePlanetMeshes(planetControl.CreateSphereMeshDraft(700,700));
            continentTree.UpdatePlanetMeshColliders(planetControl.CreateSphereMeshDraft(100,100));
        }
    }
}