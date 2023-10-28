using Common;
using Common.Model;
using Common.Model.Abstractions;
using Common.Model.Query;
using Common.Utils;
using Game.Common.Holder;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace InGameUi.InputFiller
{
    public class CalendarListElementFiller : InputFiller<CalendarModelIdHolder, Calendar, IdOf<Calendar>, GetCalendarById>
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text yearPhasesText;
        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), nameText, descriptionText, yearPhasesText);
        }
        protected override IdOf<Calendar> GetQueryParam(CalendarModelIdHolder component) => component.Value;

        protected override void SetValue(CalendarModelIdHolder component, Calendar value)
        {
            nameText.text = value.Name;
            descriptionText.text = value.Description;
            yearPhasesText.text = $"Year Phases: {value.YearPhases}";
        }
    }
}