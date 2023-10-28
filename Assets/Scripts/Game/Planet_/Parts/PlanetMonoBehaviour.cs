#nullable enable
using Common;
using Common.Geometry.Sphere;
using Common.Model;
using Common.Model.Abstractions;
using Common.SceneChange;
using Common.Utils;
using Game.Common;
using Game.Continent_;
using Game.Planet_.Camera_;
using Game.Planet_.Parts.State;
using Game.Region_;
using ProceduralToolkit;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Planet_.Parts
{
    public class PlanetMonoBehaviour : MonoBehaviour, ISphere, IContinentFactory, IContinentClickedListener, IRegionClickedListener
    {
        private Option<PlanetControl> _planetControl = Option<PlanetControl>.None; // asserted in Awake
        private Option<ContinentsManager> _continentsManager = Option<ContinentsManager>.None; // asserted in Awake

        private PlanetControl PlanetControl => this.LazyInitialize(ref _planetControl);
        private ContinentsManager ContinentsManager => this.LazyInitialize(ref _continentsManager);
        private ContinentTree ContinentTree => ContinentsManager.ContinentTree;
        
        public Planet Planet { get; set; }
        public Transform Transform => transform;
        public float EdgeDistanceFromCamera => Transform.EdgeDistanceFromCamera(MainCamera);
        public Camera MainCamera => PlanetControl.MainCamera;

        public ContinentsState State => ContinentsManager.State;

        public UnityEvent<IContinentState, IContinentState> StateChanged => ContinentsManager.StateChanged;

        private void Start()
        {
            SceneChangeParameters
                .NonNullInstance(GetType())
                .GetNonNullable(SceneParamKeys.WorldInitializeParams)
                .DoIfNotNull(ToEditPlanetStateInitially);
        }

        public MeshDraft CreateSphereMeshDraft(int horizontalSegments, int verticalSegments) =>
            PlanetControl.CreateSphereMeshDraft(horizontalSegments, verticalSegments);


        internal void SetContinentState(IContinentState continentState) =>
            ContinentsManager.SetContinentState(continentState);
        
        public void SelectContinent(ContinentMonoBehaviour continent) => ContinentsManager.SelectContinent(continent, this);
        public void SelectRegion(RegionMonoBehaviour region) => ContinentsManager.SelectRegion(region, this);

        public void StartCreatingNewContinent() => ContinentsManager.StartCreatingNewContinent();
        public void MakeAllLinesInvisible() => ContinentsManager.MakeAllLinesInvisible();
        public void MakeAllLinesInvisibleExcept(Option<BoundedLocationMono> continent) =>
            ContinentsManager.MakeAllLinesInvisibleExcept(continent);

        public TResult ApplyStateOperation<TState, TResult>(IContinentStateOperation<TState, TResult> operation)
            where TState : IContinentState => ContinentsManager.ApplyStateOperation(operation);
        
        public void ApplyStateOperation<TState>(IContinentStateOperation<TState> operation)
            where TState : IContinentState => ContinentsManager.ApplyStateOperation(operation);
        public void UpdatePlanetLines() => ContinentsManager.UpdatePlanetLines();
        public void ToEditPlanetState() => ContinentsManager.ToEditPlanetState(this);
        public void StartCreatingNewRegion() => ContinentsManager.StartCreatingNewRegion();

        public void ToEditPlanetStateInitially(PlanetWithRelatedEntities worldInitParams)
        {
            Planet = worldInitParams.Planet;
            ContinentsManager.ToEditPlanetStateInitially(worldInitParams, this);
        }

        public void UpdatePlanetMeshes() => ContinentsManager.UpdatePlanetMeshes();

        public ContinentMonoBehaviour CreateContinentWithParent(ContinentWithParent continent)
        {
            return ContinentTree.CreateContinentWithParent(continent, this, this);
        }

        public void OnMouseDownOverContinent(ContinentMonoBehaviour continentMonoBehaviour)
        {
            ContinentsManager.ClickedOnContinent(continentMonoBehaviour);
        }

        public void OnMouseDownOverRegion(RegionMonoBehaviour continentMonoBehaviour)
        {
            ContinentsManager.ClickedOnRegion(continentMonoBehaviour);
        }

        public ContinentMonoBehaviour FindContinent(IdOf<Continent> continentId) =>
            ContinentsManager.ContinentTree.FindNodeById(continentId).MonoBehaviour;
    }
}