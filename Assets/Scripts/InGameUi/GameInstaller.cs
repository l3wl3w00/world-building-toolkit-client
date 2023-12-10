#nullable enable
using Common.Model;
using Common.Model.Abstractions;
using Common.Model.Builder;
using Common.Utils;
using Game;
using Game.Common;
using Game.Common.Holder;
using Game.Planet_.Parts;
using GameController;
using InGameUi.Factory;
using InGameUi.InputFiller;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;


namespace InGameUi
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlanetMonoBehaviour planetMonoBehaviour = null!; // Asserted in InstallBindings
        [SerializeField] private UiController uiController = null!; // Asserted in InstallBindings
        public override void InstallBindings()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), planetMonoBehaviour, uiController);
            
            Container.Bind<PlanetMonoBehaviour>().FromInstance(planetMonoBehaviour).AsSingle();
            Container.Bind<UiController>().FromInstance(uiController).AsSingle();

            Container.Bind<ModelCollection<HistoricalEvent>>().AsSingle();
            Container.Bind<ModelCollection<Calendar>>().AsSingle();
            Container.Bind<ModelCollection<Continent>>().AsSingle();
            Container.Bind<ModelCollection<Region>>().AsSingle();

            Container.Bind<CalendarBuilderHolder>().AsSingle();
            Container.Bind<HistoricalEventBuilderHolder>().AsSingle();
            Container.Bind<RegionBuilderHolder>().AsSingle();
            Container.Bind<PlanetBuilderHolder>().AsSingle();
            Container.Bind<ContinentBuilderHolder>().AsSingle();
            Container.BindFactory<IdOf<Calendar>, Transform, GameObject, CalendarUiItemFactory>()
                .FromFactory<CalendarUiItemFactory.Logic>();

            Container.BindFactory<IdOf<Calendar>, Transform, GameObject, CalendarUiItemForEventFactory>()
                .FromFactory<CalendarUiItemForEventFactory.Logic>();
            
            Container.BindFactory<IdOf<HistoricalEvent>,GameObject, HistoricalEventUiDetailedFactory>()
                .FromFactory<HistoricalEventUiDetailedFactory.Logic>();
            
            Container.BindFactory<IdOf<HistoricalEvent>, Transform, GameObject, HistoricalEventUiSummaryFactory>()
                .FromFactory<HistoricalEventUiSummaryFactory.Logic>();
            
            Container.BindFactory<Transform, GameObject, AiAssistantUiFactory>()
                .FromFactory<AiAssistantUiFactory.Logic>();
            
            Container.BindFactory<GameObject, CalendarListUiFactory>()
                .FromFactory<CalendarListUiFactory.Logic>();

            Container.BindFactory<GameObject, CalendarCreateUiFactory>()
                .FromFactory<CalendarCreateUiFactory.Logic>();
            
            Container.BindFactory<IdOf<Calendar>, GameObject, CalendarDetailedUiFactory>()
                .FromFactory<CalendarDetailedUiFactory.Logic>();
            
            Container.BindFactory<Transform, GameObject, CreateYearPhaseUiItemFactory>()
                .FromFactory<CreateYearPhaseUiItemFactory.Logic>();
            
            Container.BindFactory<ModelIdHolder<Calendar>, Transform, GameObject, YearPhaseUiItemFactory>()
                .FromFactory<YearPhaseUiItemFactory.Logic>();
            
            Container.BindFactory<GameObject, ListHistoricalEventsUiFactory>()
                .FromFactory<ListHistoricalEventsUiFactory.Logic>();
            
            Container.BindFactory<GameObject, CreateHistoricalEventUiFactory>()
                .FromFactory<CreateHistoricalEventUiFactory.Logic>();
            
            SignalBusInstaller.Install(Container);

            Container.Bind<InputUpdaterOnSignal>().FromNewComponentOnNewGameObject().AsSingle();
            Container.DeclareSignal<StateChangedSignal>();
            Container.BindSignal<StateChangedSignal>()
                .ToMethod<InputUpdaterOnSignal>(i => i.UpdateValues)
                .FromResolve();
        }
    }
}