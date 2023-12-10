#nullable enable
using System.Linq;
using Common;
using Common.Model;
using Common.Utils;
using InGameUi.Factory;
using InGameUi.Util;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InGameUi.InputFiller.Calendar_
{
    public class CalendarListFiller : MonoBehaviour, IStateObserver
    {
        [Inject] private CalendarUiItemFactory _calendarUiItemFactory = null!; //Asserted in Start
        [Inject] private ModelCollection<Calendar> _calendars = null!; //Asserted in Start

        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _calendarUiItemFactory, _calendars);
            gameObject.GetContentInChildren(); // to assert it is present
        }

        public void UpdateValue()
        {
            var content = gameObject.GetContentInChildren();
            if (content.GetChildren().Count() != _calendars.Count)
            {
                content.GetChildren().ForEach(Destroy);
                content.transform.DetachChildren();
                _calendars.ForEach(c => _calendarUiItemFactory.Create(c.Id, content.transform));
            }

            content.GetComponentsInChildren<CalendarListElementFiller>().ForEach(f => f.UpdateValue());
        }
    }
}