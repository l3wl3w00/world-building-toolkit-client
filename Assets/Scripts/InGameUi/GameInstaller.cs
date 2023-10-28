#nullable enable
using Common.Model;
using Common.Model.Abstractions;
using Common.Model.Builder;
using Common.Utils;
using Game.Planet_.Parts;
using InGameUi.Factory;
using InGameUi.InputFiller;
using UnityEngine;
using Zenject;

namespace InGameUi
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlanetMonoBehaviour planetMonoBehaviour = null!; // Asserted in InstallBindings
        public override void InstallBindings()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), planetMonoBehaviour);
            
            Container.Bind<PlanetMonoBehaviour>().FromInstance(planetMonoBehaviour).AsSingle();
            Container.Bind<ModelCollection<HistoricalEvent>>().AsSingle();
            Container.Bind<ModelCollection<Calendar>>().AsSingle();

            Container.Bind<CalendarBuilderHolder>().AsSingle();
            Container.Bind<HistoricalEventBuilderHolder>().AsSingle();
            Container.Bind<RegionBuilderHolder>().AsSingle();
            Container.Bind<PlanetBuilderHolder>().AsSingle();
            Container.Bind<ContinentBuilderHolder>().AsSingle();
            Container.BindFactory<IdOf<Calendar>, Transform, GameObject, CalendarUiItemFactory>()
                .FromFactory<CalendarUiItemFactory.Logic>();
            
            Container.BindFactory<GameObject, CalendarListUiFactory>()
                .FromFactory<CalendarListUiFactory.Logic>();

            Container.BindFactory<GameObject, CalendarCreateUiFactory>()
                .FromFactory<CalendarCreateUiFactory.Logic>();
            
            Container.BindFactory<GameObject, CalendarDetailedUiFactory>()
                .FromFactory<CalendarDetailedUiFactory.Logic>();
        }
    }
}