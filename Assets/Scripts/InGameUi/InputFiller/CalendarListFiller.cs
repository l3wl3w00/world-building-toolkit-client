#nullable enable
using System.Collections.Generic;
using Common;
using Common.Model;
using Common.Model.Abstractions;
using Common.Utils;
using Game.Common.Holder;
using Generated;
using InGameUi.Factory;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InGameUi.InputFiller
{
    public class CalendarListFiller : InputFiller<ScrollRect, ModelCollection<Calendar>>
    {
        [Inject] private CalendarUiItemFactory _calendarUiItemFactory;
        [Inject] private ModelCollection<Calendar> _calendars;
        private const string NameOfContent = "Content";
        private GameObject Content() => gameObject
            .GetChild(NameOfContent)
            .ExpectNotNull($"No GameObject named '{NameOfContent}' was present under this {GetType().Name}", gameObject);
        protected override void OnStart()
        {
            Content(); // to assert it is present
        }

        protected override Option<ModelCollection<Calendar>> GetValue(ScrollRect component) => _calendars.ToOption();

        protected override void SetValue(ScrollRect component, ModelCollection<Calendar> value)
        {
            var content = Content();
            value.ForEach(v => _calendarUiItemFactory.Create(v.Id, content.transform));
        }
    }
}