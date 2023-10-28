using Common.Model;
using Common.Model.Abstractions;
using Common.Utils;
using Game.Common.Holder;
using Generated;
using UnityEngine;
using Zenject;

namespace InGameUi.Factory
{
    public class CalendarDetailedUiFactory : PlaceholderFactory<GameObject>
    {
        public class Logic : IFactory<GameObject>
        {
            [Inject] private DiContainer _container;

            public GameObject Create() => Prefab.CalendarUIElementDetailed2.Instantiate(_container);
        }
    }
    
    public class CalendarCreateUiFactory : PlaceholderFactory<GameObject>
    {
        public class Logic : IFactory<GameObject>
        {
            [Inject] private DiContainer _container;
            public GameObject Create() => Prefab.CalendarCreateUi.Instantiate(_container);
        }
    }
    
    public class CalendarListUiFactory : PlaceholderFactory<GameObject>
    {
        public class Logic : IFactory<GameObject>
        {
            [Inject] private DiContainer _container;

            public GameObject Create()
            {
                Debug.Log("Creating CalendarListUI");
                return Prefab.CalendarListUI.Instantiate(_container);
            }
        }
    }
    
    public class CalendarUiItemFactory : PlaceholderFactory<IdOf<Calendar>, Transform, GameObject>
    {
        public class Logic : IFactory<IdOf<Calendar>, Transform, GameObject>
        {
            public GameObject Create(IdOf<Calendar> calendarId, Transform parent)
            {
                Debug.Log("Creating CalendarUiItem");
                var calendar = Prefab.CalendarUIElementSummary
                    .Instantiate(parent)
                    .GetComponent<CalendarModelIdHolder>();
                Debug.Log("Initializing CalendarUiItem");
                calendar.Value = calendarId;
                return calendar.gameObject;
            }
        }
    }

    internal static class FactoryUtils
    {
        public static GameObject Instantiate(this Prefab prefab, DiContainer container) => container.InstantiatePrefab(prefab.Load());
    }
}