#nullable enable
using Common.Model;
using Common.Utils;
using GameController.Queries;
using InGameUi.Factory;
using InGameUi.Util;
using UnityEngine;
using Zenject;

namespace InGameUi.InputFiller.Event_
{
    public class EventListFiller : MonoBehaviour, IStateObserver
    {
        [Inject] private GetEventsForSelectedRegion _getEventsForSelectedRegion;
        [Inject] private HistoricalEventUiSummaryFactory _eventUiSummaryFactory;
        public void UpdateValue()
        {
            var events = _getEventsForSelectedRegion.Get();
            var content = gameObject.GetContentInChildren();
            if (events.Count != content.transform.childCount)
            {
                content.GetChildren().ForEach(Destroy);
                content.transform.DetachChildren();
                events.ForEach(e => _eventUiSummaryFactory.Create(e.Id, content.transform));
            }
            
            content.UpdateChildInputFillers();
        }
    }
}