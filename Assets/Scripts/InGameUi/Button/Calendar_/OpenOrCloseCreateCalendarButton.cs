#nullable enable
using Common.ButtonBase;
using Common.Model.Builder;
using Common.Triggers;
using Common.Triggers.GameController;
using InGameUi.Factory;
using Zenject;

namespace InGameUi.Button.Calendar_
{
    public class OpenOrCloseCreateCalendarButton : ButtonActionTrigger<ShowCreateCalendarUiCommand>
    {
        
    }

    public class ShowCreateCalendarUiCommand : ActionListener
    {
        [Inject] private UiController _uiController;
        [Inject] private CalendarCreateUiFactory _calendarCreateUiFactory;
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder;
        public override void OnTriggered(NoActionParam param)
        {
            _uiController.OpenOrClose(UiType.CreateCalendar, _calendarCreateUiFactory.Create);
            _calendarBuilderHolder.StartBuildingModel();
        }
    }
}