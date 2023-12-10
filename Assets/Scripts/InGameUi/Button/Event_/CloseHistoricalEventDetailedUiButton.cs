#nullable enable
using Common.ButtonBase;
using Common.Triggers;
using Common.Triggers.GameController;
using Zenject;

namespace InGameUi.Button.Event_
{
    public class CloseHistoricalEventDetailedUiButton : ButtonActionTrigger<CloseHistoricalEventDetailedUiCommand>
    {
        
    }

    public class CloseHistoricalEventDetailedUiCommand : ActionListenerMono<NoActionParam>
    {
        [Inject] private UiController _uiController;
        public override void OnTriggered(NoActionParam param)
        {
            _uiController.CloseUi(UiType.SingleHistoricalEventDetailed);
        }
    }
}