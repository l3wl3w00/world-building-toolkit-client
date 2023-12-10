#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Common.Model;
using Common.Model.Builder;
using Common.Utils;
using Game;
using GameController;
using GameController.Queries;
using ModestTree;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace InGameUi.Button.Event_
{
    public class HistoricalEventActionTrigger : MonoBehaviour
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
        [Inject] private SelectedRegionQuery _selectedRegionQuery = null!; //Asserted in start
        [Inject] private SignalBus _signalBus;

        private bool _queuedForUpdate = false;
        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _eventBuilderHolder, _calendars, _selectedRegionQuery);
            calendarDropdown.onValueChanged.AddListener(_ => QueueForUpdate());
            
            beginningYearPhaseDropdown.onValueChanged.AddListener(_ => QueueForUpdate());
            beginningYearInput.onValueChanged.AddListener(_ => QueueForUpdate());
            beginningDayInput.onValueChanged.AddListener(_ => QueueForUpdate());
            beginningHourInput.onValueChanged.AddListener(_ => QueueForUpdate());
            beginningMinuteInput.onValueChanged.AddListener(_ => QueueForUpdate());
            
            endYearPhaseDropdown.onValueChanged.AddListener(_ => QueueForUpdate());
            endYearInput.onValueChanged.AddListener(_ => QueueForUpdate());
            endDayInput.onValueChanged.AddListener(_ => QueueForUpdate());
            endHourInput.onValueChanged.AddListener(_ => QueueForUpdate());
            endMinuteInput.onValueChanged.AddListener(_ => QueueForUpdate());
            
            descriptionInput.onValueChanged.AddListener(_ => QueueForUpdate());
        }

        private void QueueForUpdate()
        {
            _queuedForUpdate = true;
        }

        private void Update()
        {
            if (!_queuedForUpdate) return;
            
            UpdateModel();
            _queuedForUpdate = false;
        }

        private void UpdateModel()
        {
            if (_eventBuilderHolder.IsModelNotInCreation) return;
            var eventBuilder = _eventBuilderHolder.Builder;

            if (_calendars.IsEmpty())
            {
                Debug.LogError("There were no calendars when trying to create an event");
                return;
            }

            var calendarDropdownValue = UpdateDropdownAndKeepValue(calendarDropdown, _calendars.Select(c => c.Name));
            var calendarName = calendarDropdown.options[calendarDropdownValue].text;
            var calendar = _calendars.GetByName(calendarName);
            
            // set beginning date
            var beginningYearPhaseDropdownValue = UpdateDropdownAndKeepValue(
                beginningYearPhaseDropdown, 
                calendar.YearPhases.Select(y => y.Name));
            var beginningYearPhaseName = beginningYearPhaseDropdown.options[beginningYearPhaseDropdownValue].text;
            var newBeginning = new Date(
                Year: beginningYearInput.text.ToInt(),
                YearPhase: beginningYearPhaseName,
                Day: beginningDayInput.text.ToUInt(),
                Hour: beginningHourInput.text.ToUInt(),
                Minute: beginningMinuteInput.text.ToUInt());

            // set end date
            var endYearPhaseDropdownValue =
                UpdateDropdownAndKeepValue(endYearPhaseDropdown, calendar.YearPhases.Select(y => y.Name));
            var endYearPhaseName = endYearPhaseDropdown.options[endYearPhaseDropdownValue].text;
            var newEnd = new Date(
                Year: endYearInput.text.ToInt(),
                YearPhase: endYearPhaseName,
                Day: endDayInput.text.ToUInt(),
                Hour: endHourInput.text.ToUInt(),
                Minute: endMinuteInput.text.ToUInt());
            
            eventBuilder
                .WithBeginning(newBeginning)
                .WithEnd(newEnd)
                .WithDescription(descriptionInput.text)
                .WithDefaultCalendar(calendar.Id)
                .WithName(nameInput.text)
                .WithRegion(_selectedRegionQuery.Get().Id);
        }

        private int UpdateDropdownAndKeepValue(TMP_Dropdown dropdown, IEnumerable<string> values)
        {
            var calendarDropdownValue = dropdown.value;
            dropdown.ClearOptions();
            dropdown.AddOptions(values.ToList());
            dropdown.value = calendarDropdownValue;
            return calendarDropdownValue;
        }
    }
}