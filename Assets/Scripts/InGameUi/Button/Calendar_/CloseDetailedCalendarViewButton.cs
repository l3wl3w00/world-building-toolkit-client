#nullable enable
using Common.ButtonBase;
using Common.Triggers;
using Zenject;

namespace InGameUi.Button.Calendar_
{
    public class CloseDetailedCalendarViewButton : ButtonActionTrigger<CloseDetailedCalendarViewCommand>
    {
    }
    public class CloseDetailedCalendarViewCommand : ActionListener
    {
        [Inject] private UiController _uiController;
        public override void OnTriggered(NoActionParam param)
        {
            _uiController.CloseUi(UiType.SingleCalendarDetailed);
        }
    }
}