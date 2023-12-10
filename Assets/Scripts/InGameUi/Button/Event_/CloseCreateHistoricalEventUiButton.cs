#nullable enable
using Common.ButtonBase;
using Common.Model.Builder;
using Common.Triggers;
using Common.Triggers.GameController;
using Zenject;

namespace InGameUi.Button.Event_
{
    public class CloseCreateHistoricalEventUiButton : ButtonActionTrigger<CloseCreateHistoricalEventUiCommand>
    {
        
    }

    public class CloseCreateHistoricalEventUiCommand : ActionListener
    {
        [Inject] private UiController _uiController;
        [Inject] private HistoricalEventBuilderHolder _builderHolder;
        public override void OnTriggered(NoActionParam param)
        {
            _builderHolder.CancelCreation();
            _uiController.CloseUi(UiType.CreateHistoricalEvent);
        }
    }
}