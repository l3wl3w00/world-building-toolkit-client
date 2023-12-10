#nullable enable
using System;
using Common.Model;
using Common.Model.Abstractions;
using Common.Utils;
using Game.Common;
using Game.Common.Holder;
using InGameUi.Factory;
using InGameUi.InputFiller.Calendar_;
using TMPro;
using UnityEngine;
using Zenject;

namespace InGameUi.InputFiller.Event_
{
    public class HistoricalEventDetailedFiller : MonoBehaviour, IStateObserver
    {
        [Inject] private ModelCollection<HistoricalEvent> _events;
        [Inject] private ModelCollection<Calendar> _calendars;

        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text startDateText;
        [SerializeField] private TMP_Text endDateText;
        [SerializeField] private Transform calendarUiParent;

        public void Start()
        {
            var uiElementFiller = this.AssertComponentInChildren<CalendarListElementFiller>();
            uiElementFiller.Construct(_calendars);
            if (GetEvent(out var hEvent)) return;
            uiElementFiller.AssertComponentInChildren<CalendarModelIdHolder>().Value = hEvent!.DefaultCalendar;
        }

        public void UpdateValue()
        {
            if (GetEvent(out var hEvent)) return;

            var uiElementFiller = this.AssertComponentInChildren<CalendarListElementFiller>();
            uiElementFiller.AssertComponentInChildren<CalendarModelIdHolder>().Value = hEvent!.DefaultCalendar;

            nameText.text = hEvent.Name;
            descriptionText.text = hEvent.Description;
            startDateText.text = hEvent.Beginning.ToDisplayText();
            endDateText.text = hEvent.End.ToDisplayText();
        }

        private bool GetEvent(out HistoricalEvent hEvent)
        {
            var hEventOpt = gameObject.GetIdOf<HistoricalEvent>().GetModelOpt(_events);
            if (hEventOpt.NoValueOut(out hEvent))
            {
                Debug.LogWarning("Event was not found with given id, but was tried to be displayed");
                return true;
            }

            return false;
        }
    }
}