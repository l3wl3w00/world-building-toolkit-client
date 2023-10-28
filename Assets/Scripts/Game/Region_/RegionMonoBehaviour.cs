#nullable enable
using System;
using Common;
using Common.Geometry.Sphere;
using Common.Model;
using Common.Utils;
using Game.Common;
using Game.Continent_;
using Generated;
using ProceduralToolkit;
using UnityEngine;

namespace Game.Region_
{
    // methods
    public partial class RegionMonoBehaviour : BoundedLocationMono
    {
        private void OnMouseDown() => _clickedListener.OnMouseDownOverRegion(this);
        private void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _clickedListener);
        }

        public void UpdateMesh(MeshDraft draft)
        {
            PartialSphereMeshHandler.UpdateMesh(draft.ToMesh(), Region.Color);
        }
        
        
        public void OnColorChanged(Color color)
        {
            Region = Region with { Color = color };
            UpdateColor();
        }

        public void UpdateColor()
        {
            MeshRenderer.material.color = (Region.Color + new Color(_addition,_addition,_addition,_addition)) * _multiplier;
        }

        private void OnMouseEnter()
        {
            _multiplier = 1.1f;
            _addition = 0.1f;
        }

        private void OnMouseExit()
        {
            _multiplier = 1f;
            _addition = 0f;
        }

        private void Update()
        {
            UpdateColor();
        }
    }

    // initialization stuff
    public partial class RegionMonoBehaviour : IGameObjectFactoryMethod<
        RegionMonoBehaviour.CreateParams,
        RegionMonoBehaviour.RegionProvider,
        RegionMonoBehaviour>
    {
        private record RegionProvider() : PrefabProvider(Prefab.Region);
        public record CreateParams(ISphere sphere, Region region, IRegionClickedListener clickedListener) : CreateParamsFlag;

        public static RegionMonoBehaviour Create(CreateParams p) =>
            IGameObjectFactoryMethod<CreateParams, RegionProvider, RegionMonoBehaviour>.Create(p, p.sphere.Transform);
        public void Initialize(CreateParams createParams)
        {
            Region = createParams.region;
            _clickedListener = createParams.clickedListener;
            SphereControlPointsHandler.Initialize(Region.GlobalBounds, createParams.sphere);
            SphereLineRendererHandler.Initialize(createParams.sphere);
            MeshRenderer.material.color = Region.Color;
            UpdateColor();
        }
    }

    // properties and fields
    public partial class RegionMonoBehaviour
    {
        private static long _continentCounter;
        private IRegionClickedListener _clickedListener = null!; // Asserted in Start
        private Option<RegionModelHolder> _regionModelHolder = Option<RegionModelHolder>.None;
        private Option<MeshRenderer> _meshRenderer = Option<MeshRenderer>.None;
        private float _multiplier = 1f;
        private float _addition = 0f;

        public bool Inverted => Region.Inverted;
        private RegionModelHolder RegionModelHolder => this.LazyInitialize(ref _regionModelHolder);
        private MeshRenderer MeshRenderer => this.LazyInitialize(ref _meshRenderer);
        public Region Region
        {
            get => RegionModelHolder.Value;
            set => RegionModelHolder.Value = value;
        }
    }
    
    public interface IRegionClickedListener
    {
        void OnMouseDownOverRegion(RegionMonoBehaviour continentMonoBehaviour);
    }
}