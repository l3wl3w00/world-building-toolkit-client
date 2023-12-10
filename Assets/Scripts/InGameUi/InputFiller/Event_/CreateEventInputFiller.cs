#nullable enable
using System.Collections.Generic;
using System.Linq;
using Common.Model;
using Common.Model.Abstractions;
using Common.Model.Builder;
using TMPro;
using UnityEngine;
using Zenject;

namespace InGameUi.InputFiller.Event_
{
    public class CreateEventInputFiller : MonoBehaviour, IStateObserver
    {
        [SerializeField] private TMP_Dropdown calendarDropdown;
        
        [SerializeField] private TMP_Dropdown beginningYearPhaseDropdown;
        [SerializeField] private TMP_InputField beginningYearInput;
        [SerializeField] private TMP_InputField beginningDayInput;
        [SerializeField] private TMP_InputField beginningHourInput;
        [SerializeField] private TMP_InputField beginningMinuteInput;
        
        [SerializeField] private TMP_Dropdown endYearPhaseDropdown;
        [SerializeField] private TMP_InputField endYearInput;
        [SerializeField] private TMP_InputField endDayInput;
        [SerializeField] private TMP_InputField endHourInput;
        [SerializeField] private TMP_InputField endMinuteInput;
        
        [SerializeField] private TMP_InputField descriptionInput;
        [SerializeField] private TMP_InputField nameInput;

        [Inject] private HistoricalEventBuilderHolder _eventBuilderHolder = null!; //Asserted in start
        [Inject] private ModelCollection<Calendar> _calendars = null!; //Asserted in start
        
        private void SelectCalendarInCalendarDropdown(Calendar calendar)
        {
            var selectedCalendar = calendarDropdown.options.FindIndex(o => o.text == calendar.Name);
            calendarDropdown.value = selectedCalendar;
        }

        private void UpdateCalendarOptions()
        {
            calendarDropdown.ClearOptions();
            calendarDropdown.AddOptions(_calendars.Select(c => c.Name).ToList());
        }
        
        private void UpdateYearPhaseOptions(TMP_Dropdown yearPhaseDropdown, IEnumerable<YearPhase> yearPhases)
        {
            yearPhaseDropdown.ClearOptions();
            yearPhaseDropdown.AddOptions(yearPhases.Select(y => y.Name).ToList());
        }
        
        private void SelectYearPhase(TMP_Dropdown yearPhaseDropdown, string yearPhaseName)
        {
            var selectedYearPhase = yearPhaseDropdown.options.FindIndex(o => o.text == yearPhaseName);
            yearPhaseDropdown.value = selectedYearPhase;
        }

        public void UpdateValue()
        {
            if (_eventBuilderHolder.IsModelNotInCreation) return;
            var eventBuilder = _eventBuilderHolder.Builder;
            
            var calendar = eventBuilder.DefaultCalendar.GetModel(_calendars);
            UpdateCalendarOptions();
            UpdateYearPhaseOptions(beginningYearPhaseDropdown, calendar.YearPhases);
            UpdateYearPhaseOptions(endYearPhaseDropdown, calendar.YearPhases);
            
            SelectYearPhase(endYearPhaseDropdown, eventBuilder.Beginning.YearPhase);
            SelectYearPhase(beginningYearPhaseDropdown, eventBuilder.End.YearPhase);

            beginningYearInput.text = eventBuilder.Beginning.Year.ToString();
            beginningDayInput.text = eventBuilder.Beginning.Day.ToString();
            beginningHourInput.text = eventBuilder.Beginning.Hour.ToString();
            beginningMinuteInput.text = eventBuilder.Beginning.Minute.ToString();
            endYearInput.text = eventBuilder.End.Year.ToString();
            endDayInput.text = eventBuilder.End.Day.ToString();
            endHourInput.text = eventBuilder.End.Hour.ToString();
            endMinuteInput.text = eventBuilder.End.Minute.ToString();
            descriptionInput.text = eventBuilder.Description;
            nameInput.text = eventBuilder.Name;
        }
    }
}