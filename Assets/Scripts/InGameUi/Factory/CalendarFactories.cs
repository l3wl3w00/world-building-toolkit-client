#nullable enable
using Common;
using Common.Generated;
using Common.Model;
using Common.Model.Abstractions;
using Common.Utils;
using Game.Common;
using Game.Common.Holder;
using InGameUi.InputFiller.Calendar_;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace InGameUi.Factory
{
    public class CalendarDetailedUiFactory : PlaceholderFactory<IdOf<Calendar>, GameObject>
    {
        public class Logic : IFactory<IdOf<Calendar>, GameObject>
        {
            [Inject] private DiContainer _container;
            [Inject] private UiController _uiController;

            public GameObject Create(IdOf<Calendar> id) => 
                Prefab.CalendarUIElementDetailed.InstantiateModel(_container, _uiController.transform, id);
        }
    }
    
    public class CalendarCreateUiFactory : PlaceholderFactory<GameObject>
    {
        public class Logic : IFactory<GameObject>
        {
            [Inject] private DiContainer _container;
            [Inject] private UiController _uiController;
            public GameObject Create() => Prefab.CalendarCreateUi.Instantiate(_container, _uiController.transform);
        }
    }
    
    public class CalendarListUiFactory : PlaceholderFactory<GameObject>
    {
        public class Logic : IFactory<GameObject>
        {
            [Inject] private DiContainer _container;
            [Inject] private UiController _uiController;

            public GameObject Create()
            {
                Debug.Log("Creating CalendarListUI");
                return Prefab.CalendarListUI.Instantiate(_container, _uiController.transform);
            }
        }
    }
    
    public class CalendarUiItemFactory : PlaceholderFactory<IdOf<Calendar>, Transform, GameObject>
    {
        public class Logic : IFactory<IdOf<Calendar>, Transform, GameObject>
        {
            [Inject] private DiContainer _container;

            public GameObject Create(IdOf<Calendar> calendarId, Transform parent) =>
                Prefab.CalendarUIElementSummary.InstantiateModel(_container, parent, calendarId);
        }
    }
    
    public class CalendarUiItemForEventFactory : PlaceholderFactory<IdOf<Calendar>, Transform, GameObject>
    {
        public class Logic : IFactory<IdOf<Calendar>, Transform, GameObject>
        {
            [Inject] private DiContainer _container;

            public GameObject Create(IdOf<Calendar> calendarId, Transform parent) =>
                Prefab.CalendarUIElementSummaryForEvent.InstantiateModel(_container, parent, calendarId);
        }
    }
    
    public class CreateYearPhaseUiItemFactory : PlaceholderFactory<Transform, GameObject>
    {
        public class Logic : IFactory<Transform, GameObject>
        {
            [Inject] private DiContainer _container;

            public GameObject Create(Transform parent) =>
                Prefab.CreateYearPhaseUi.Instantiate(_container, parent);
        }
    }
    
    public class YearPhaseUiItemFactory : PlaceholderFactory<ModelIdHolder<Calendar>, Transform, GameObject>
    {
        public class Logic : IFactory<ModelIdHolder<Calendar>, Transform, GameObject>
        {
            [Inject] private DiContainer _container;

            public GameObject Create(ModelIdHolder<Calendar> idHolder, Transform parent) =>
                Prefab.YearPhaseUi
                    .InstantiateAndExpectComponent<YearPhaseUiFiller>(_container, parent)
                    .Apply(yp => yp.Construct(idHolder))
                    .gameObject;
        }
    }

    internal static class FactoryUtils
    {
        public static GameObject Instantiate(this Prefab prefab, DiContainer container, Transform parent)
        {
            var r1 = container.InstantiatePrefab(prefab.Load());
            r1.transform.SetParent(parent);
            return r1;
        }
        
        public static GameObject InstantiateModel<T>(this Prefab prefab, DiContainer container, Transform parent, IdOf<T> modelId) 
            where T : IModel<T> =>
            prefab.InstantiateAndExpectComponent<ModelIdHolder<T>>(container, parent)
                .Apply(h => h.Value = modelId)
                .gameObject;

        public static T InstantiateAndExpectComponent<T>(this Prefab prefabName, DiContainer container, Transform parent)
            where T : Component =>
            prefabName.Instantiate(container, parent)
                .GetComponent<T>()
                .ToOption()
                .ExpectNotNull($"Prefab {prefabName.Name} does not contain a {typeof(T).Name} on its root game object, but is expected to");
    }
}