#nullable enable
using Common.ButtonBase;
using Common.Model.Builder;
using Common.Triggers;
using Common.Triggers.GameController;
using Common.Utils;
using Zenject;

namespace InGameUi.Button.Calendar_
{
    public class CancelCreatingCalendarButton : ButtonActionTrigger<CancelCreatingCalendarCommand>
    {
        
    }

    public class CancelCreatingCalendarCommand : ActionListener
    {
        [Inject] private UiController _uiController = null!; // Asserted in Start
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder = null!; // Asserted in Start

        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _uiController, _calendarBuilderHolder);
        }
        public override void OnTriggered(NoActionParam param)
        {
            _uiController.CloseUi(UiType.CreateCalendar);
            _calendarBuilderHolder.CancelCreation();
        }
    }
}