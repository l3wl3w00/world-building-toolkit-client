#nullable enable
using Common.ButtonBase;
using Common.Triggers;
using Zenject;

namespace InGameUi.Button.Event_
{
    public class CloseHistoricalEventListUiButton : ButtonActionTrigger<CloseHistoricalEventListUiCommand>
    {
        
    }
    
    public class CloseHistoricalEventListUiCommand : ActionListener
    {
        [Inject] private UiController _uiController;
        public override void OnTriggered(NoActionParam param)
        {
            _uiController.CloseUi(UiType.ListHistoricalEvents);
        }
    }
}