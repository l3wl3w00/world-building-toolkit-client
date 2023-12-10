#nullable enable
using Common.Model;
using Common.Model.Abstractions;
using Game.Common;
using TMPro;
using UnityEngine;
using Zenject;

namespace InGameUi.InputFiller.Event_
{
    public class HistoricalEventSummaryFiller : MonoBehaviour, IStateObserver
    {
        [SerializeField] private TMP_Text summaryText;
        [Inject] private ModelCollection<HistoricalEvent> _events;
        [Inject] private ModelCollection<Calendar> _calendars;

        public void UpdateValue()
        {
            summaryText.text = gameObject.GetIdOf<HistoricalEvent>().GetModel(_events).ToDisplayText(_calendars);
        }
    }
}