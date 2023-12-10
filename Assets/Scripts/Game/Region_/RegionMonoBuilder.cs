#nullable enable
using System.Collections.Generic;
using Common.Generated;
using Common.Geometry.Sphere;
using Common.Model;
using Common.Model.Abstractions;
using Game.Common;
using UnityEngine;

namespace Game.Region_
{
    public class RegionMonoBuilder : BoundedLocationMono,
        IGameObjectFactoryMethod<RegionMonoBuilder.CreateParams,RegionMonoBuilder.RegionBuilder,RegionMonoBuilder>
    {
        private bool _finishedBuilding = false;
        public IRegionClickedListener ClickedListener { get; set; }

        public ISphere Sphere { get; set; }


        public record CreateParams(IdOf<Continent> continentId, ISphere sphere, IRegionClickedListener clickedListener) : CreateParamsFlag;

        private record RegionBuilder() : PrefabProvider(Prefab.RegionBuilder);

        public static RegionMonoBuilder Create(CreateParams p) =>
            IGameObjectFactoryMethod<CreateParams, RegionBuilder, RegionMonoBuilder>.Create(p, p.sphere.Transform);
        

        public void Initialize(CreateParams createParams)
        {
            ContinentId = createParams.continentId;
            Sphere = createParams.sphere;
            ClickedListener = createParams.clickedListener;
            SphereControlPointsHandler.Initialize(new List<SphereSurfaceCoordinate>(), createParams.sphere);
            SphereLineRendererHandler.Initialize(createParams.sphere);
        }

        public void AddControlPoint(Vector3 intersection)
        {
            if (_finishedBuilding)
            {
                Debug.Log("Tried to add a control point after already finished building");
                return;
            }
            SphereControlPointsHandler.AddIntersection(intersection);
            SphereControlPointsHandler.ReCreateGlobalPoints();
            SphereLineRendererHandler.UpdateLineRenderer();
        }

        public RegionMonoBehaviour Build(Ref<Region> region)
        {
            OnFinishedBuilding();
            return RegionMonoBehaviour.Create(new(Sphere, region ,ClickedListener));
        }

        public void OnFinishedBuilding()
        {
            _finishedBuilding = true;
            SphereControlPointsHandler.ConnectLineEnds();
            UpdateLines();
        }
        
        public IdOf<Continent> ContinentId { get; set; }
        public List<SphereSurfaceCoordinate> ControlPoints => SphereControlPointsHandler.ControlPoints;
    }

}