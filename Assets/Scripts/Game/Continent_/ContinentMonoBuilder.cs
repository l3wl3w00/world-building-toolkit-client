#nullable enable
using System.Collections.Generic;
using Common;
using Common.Geometry.Sphere;
using Common.Model;
using Common.Model.Abstractions;
using Common.Utils;
using Game.Common;
using Generated;
using UnityEngine;

namespace Game.Continent_
{
    public interface IContinentFactory
    {
        ContinentMonoBehaviour CreateContinentWithParent(ContinentWithParent continent);
    }

    // methods
    public partial class ContinentMonoBuilder : BoundedLocationMono 
    {
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

        public ContinentMonoBehaviour Build(ContinentWithParent continent)
        {
            return ContinentFactory.CreateContinentWithParent(continent);
        }

        public void OnFinishedBuilding()
        {
            _finishedBuilding = true;
            SphereControlPointsHandler.ConnectLineEnds();
            UpdateLines();
        }

    }

    // initialization stuff
    public partial class ContinentMonoBuilder :
        IGameObjectFactoryMethod<
            ContinentMonoBuilder.CreateParams,
            ContinentMonoBuilder.PrefabProvider,
            ContinentMonoBuilder>
    {
        private record PrefabProvider() : Common.PrefabProvider(Prefab.ContinentBuilder);
        public record CreateParams(IdOf<Continent> parentId, IContinentFactory continentFactory, ISphere sphere) : CreateParamsFlag;

        public static ContinentMonoBuilder Create(CreateParams p) =>
            IGameObjectFactoryMethod<CreateParams, PrefabProvider, ContinentMonoBuilder>.Create(p, p.sphere.Transform);

        public void Initialize(CreateParams createParams)
        {
            ParentId = createParams.parentId;
            ContinentFactory = createParams.continentFactory;
            SphereControlPointsHandler.Initialize(new List<SphereSurfaceCoordinate>(), createParams.sphere);
            SphereLineRendererHandler.Initialize(createParams.sphere);
        }
    }

    // properties and fields
    public partial class ContinentMonoBuilder
    {
        private bool _finishedBuilding = false;

        public IdOf<Continent> ParentId { get; private set; }
        public IContinentFactory ContinentFactory { get; private set; } = null!;
        public List<SphereSurfaceCoordinate> ControlPoints => SphereControlPointsHandler.ControlPoints;

    }
}