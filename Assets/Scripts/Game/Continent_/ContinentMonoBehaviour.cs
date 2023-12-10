#nullable enable
using System;
using System.Collections.Generic;
using Common;
using Common.Geometry.Sphere;
using Common.Model;
using Common.Utils;
using Game.Common;
using Common.Generated;
using ProceduralToolkit;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Continent_
{
    public partial class ContinentMonoBehaviour : BoundedLocationMono, 
            IGameObjectFactoryMethod<
                ContinentMonoBehaviour.CreateParams,
                ContinentMonoBehaviour.PrefabProvider,
                ContinentMonoBehaviour>
    {
        public record PrefabProvider() : Common.PrefabProvider(Prefab.Continent);
        public record CreateParams(ISphere sphere, Continent continent, IContinentClickedListener clickedListener) : CreateParamsFlag;

        public static ContinentMonoBehaviour Create(CreateParams p) =>
            IGameObjectFactoryMethod<CreateParams, PrefabProvider, ContinentMonoBehaviour>.Create(p, p.sphere.Transform);
        public void Initialize(CreateParams createParams)
        {
            Continent = createParams.continent;
            SphereControlPointsHandler.Initialize(Continent.GlobalBounds, createParams.sphere);
            SphereLineRendererHandler.Initialize(createParams.sphere);
            name = $"Continent{_continentCounter++}({Continent.Id})";
            _clickedListener = createParams.clickedListener;
        }

        public bool Inverted => Continent.Inverted;

        private static long _continentCounter;

        private IContinentClickedListener _clickedListener = null!; // Asserted in Start

        public Continent Continent
        {
            get => ContinentModelHolder.Value;
            set => ContinentModelHolder.Value = value;
        }

        private void OnMouseDown() => _clickedListener.OnMouseDownOverContinent(this);

        private void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _clickedListener);
        }

        public void UpdateMesh(MeshDraft draft, int depth)
        {
            var color = new Color(0.3f,0.55f,0.2f);
            if (depth % 2 == 0)
            {
                color = new Color(0.15f,0.5f,0.9f);
            }
            PartialSphereMeshHandler.UpdateMesh(draft.ToMesh(), color);
        }
        public void UpdateMeshCollider(MeshDraft draft)
        {
            PartialSphereMeshHandler.UpdateMeshCollider(draft.ToMesh());
        }
    }

    public partial class ContinentMonoBehaviour
    {
        private Option<ContinentModelHolder> _continentModelHolder = Option<ContinentModelHolder>.None;

        private ContinentModelHolder ContinentModelHolder =>
            this.LazyInitialize(ref _continentModelHolder);
    }

    public interface IContinentClickedListener
    {
        void OnMouseDownOverContinent(ContinentMonoBehaviour continentMonoBehaviour);
    }
}