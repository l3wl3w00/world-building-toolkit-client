#nullable enable
using Common.Generated;
using Common.Model;
using Common.Model.Abstractions;
using Game.Common.Holder;
using UnityEngine;
using Zenject;

namespace InGameUi.Factory
{
    public class ListHistoricalEventsUiFactory : PlaceholderFactory<GameObject>
    {
        public class Logic : IFactory<GameObject>
        {
            [Inject] private DiContainer _container;
            [Inject] private UiController _uiController;

            public GameObject Create() => 
                Prefab
                    .EventList
                    .Instantiate(_container, _uiController.transform)
                    .gameObject;
        }
    }
    
    public class CreateHistoricalEventUiFactory : PlaceholderFactory<GameObject>
    {
        public class Logic : IFactory<GameObject>
        {
            [Inject] private DiContainer _container;
            [Inject] private UiController _uiController;

            public GameObject Create() => 
                Prefab
                    .CreateEvent
                    .Instantiate(_container, _uiController.transform)
                    .gameObject;
        }
    }
    
    public class HistoricalEventUiDetailedFactory : PlaceholderFactory<IdOf<HistoricalEvent>, GameObject>
    {
        public class Logic : IFactory<IdOf<HistoricalEvent>, GameObject>
        {
            [Inject] private DiContainer _container;
            [Inject] private UiController _uiController;

            public GameObject Create(IdOf<HistoricalEvent> id) =>
                Prefab.EventDetailed.InstantiateModel(_container, _uiController.transform, id);
        }
    }
    
    public class HistoricalEventUiSummaryFactory : PlaceholderFactory<IdOf<HistoricalEvent>, Transform, GameObject>
    {
        public class Logic : IFactory<IdOf<HistoricalEvent>, Transform, GameObject>
        {
            [Inject] private DiContainer _container;

            public GameObject Create(IdOf<HistoricalEvent> id, Transform parent) =>
                Prefab.EventSummary.InstantiateModel(_container, parent, id);
        }
    }
}