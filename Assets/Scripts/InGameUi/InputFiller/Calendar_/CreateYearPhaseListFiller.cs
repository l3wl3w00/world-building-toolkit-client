#nullable enable
using System.Linq;
using Common;
using Common.Model.Builder;
using Common.Utils;
using InGameUi.Factory;
using UnityEngine;
using Zenject;

namespace InGameUi.InputFiller.Calendar_
{
    public class CreateYearPhaseListFiller : InputFiller<CanvasRenderer, CalendarBuilder>
    {
        private const string NameOfContent = "Content";
        
        [Inject] private CreateYearPhaseUiItemFactory _factory = null!; // OnStart
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder = null!; // OnStart
        [Inject] private DiContainer _container = null!; // OnStart

        private GameObject Content() => gameObject
            .GetChild(NameOfContent)
            .ExpectNotNull($"No GameObject named '{NameOfContent}' was present under this {GetType().Name}", gameObject);
        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _factory, _calendarBuilderHolder, _container);
            Content(); // to assert it is present
        }

        protected override Option<CalendarBuilder> GetValue(CanvasRenderer component) => _calendarBuilderHolder.BuilderOpt;
        
        protected override void SetValue(CanvasRenderer component, CalendarBuilder value)
        {
            var content = Content();
            if (content.GetChildren().Count() != value.YearPhases.Count)
            {
                content.GetChildren().ForEach(c => c.name = "destroyed");
                content.GetChildren().ForEach(c => c.SetActive(false));
                content.GetChildren().ForEach(Destroy);
                content.transform.DetachChildren();

                value.YearPhases.ForEach(_ => _factory.Create(content.transform));
            }
            
            content.GetComponentsInChildren<CreateYearPhaseFiller>().ForEach(f => f.UpdateValue());
        }
    }
}