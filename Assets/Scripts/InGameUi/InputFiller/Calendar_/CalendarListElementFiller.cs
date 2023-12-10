#nullable enable
using System.Reflection.Emit;
using Common.Model;
using Common.Model.Abstractions;
using Common.Utils;
using Game.Common;
using TMPro;
using UnityEngine;
using Zenject;

namespace InGameUi.InputFiller.Calendar_
{
    public class CalendarListElementFiller : MonoBehaviour, IStateObserver
    {
        [SerializeField] private TMP_Text nameText = null!; //Asserted in Start
        [SerializeField] private TMP_Text descriptionText = null!; //Asserted in Start
        [SerializeField] private TMP_Text yearPhasesText = null!; //Asserted in Start
        private ModelCollection<Calendar> _calendars = null!; //Asserted in Start

        [Inject]
        public void Construct(ModelCollection<Calendar> calendars)
        {
            _calendars = calendars;
        }

        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), nameText, descriptionText, yearPhasesText, _calendars);
        }

        public void UpdateValue()
        {
            var idHolder = gameObject.GetModelIdHolder<Calendar>();
            if (idHolder.ValueOpt.NoValueOut(out var calendarId))
            {
                Debug.LogWarning($"Tried to update the value of a {nameof(CalendarListElementFiller)} on game object '{name}', but it had no model id value");
                return;
            }
            var calendarOpt = calendarId!.GetModelOpt(_calendars);
            if (calendarOpt.NoValueOut(out var calendar)) return;
            nameText.text = calendar!.Name;
            descriptionText.text = calendar.Description;
            yearPhasesText.text = $"Year Phases: {calendar.YearPhases.Count}";
        }
    }
}