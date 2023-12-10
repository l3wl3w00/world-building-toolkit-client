#nullable enable
using Common.ButtonBase;
using Common.Triggers;
using InGameUi.Factory;
using Zenject;

namespace InGameUi.Button.Event_
{
    public class OpenOrCloseHistoricalEventListUiButton : ButtonActionTrigger<OpenOrCloseHistoricalEventListUiCommand>
    {
        
    }

    public class OpenOrCloseHistoricalEventListUiCommand : ActionListener
    {
        [Inject] private UiController _uiController;
        [Inject] private ListHistoricalEventsUiFactory _factory;
        public override void OnTriggered(NoActionParam param)
        {
            _uiController.OpenOrClose(UiType.ListHistoricalEvents, _factory.Create);
        }
    }
}