#nullable enable
using Common;
using Common.ButtonBase;
using Common.Model.Builder;
using Common.Triggers;
using GameController;
using InGameUi.Factory;
using UnityEngine;
using Zenject;

namespace InGameUi.Button.Calendar_
{
    public class OpenOrCloseListCalendarsButton : ButtonActionTrigger<OpenOrCloseCalendarListUiCommand>
    {
    }
    
    public class OpenOrCloseCalendarListUiCommand : ActionListener
    {
        [Inject] private CalendarListUiFactory _factory;
        [Inject] private UiController _uiController;
        [Inject] private SignalBus _signalBus;
        public override void OnTriggered(NoActionParam param)
        {
            _uiController.OpenOrClose(UiType.ListCalendar, _factory.Create);
        }
    }
}