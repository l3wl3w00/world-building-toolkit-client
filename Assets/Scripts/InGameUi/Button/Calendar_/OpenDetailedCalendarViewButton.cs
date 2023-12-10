#nullable enable
using Common;
using Common.ButtonBase;
using Common.Model;
using Common.Model.Abstractions;
using Common.Triggers;
using Common.Triggers.GameController;
using Game.Common;
using Game.Common.Holder;
using InGameUi.Factory;
using Zenject;

namespace InGameUi.Button.Calendar_
{
    public class OpenDetailedCalendarViewButton : UserControlledActionTrigger<UnityEngine.UI.Button>
    {
        [Inject] private OpenDetailedCalendarViewCommand _command;
        protected override void RegisterListener(UnityEngine.UI.Button component)
        {
            var calendarId = gameObject.GetIdOf<Calendar>();
            component.onClick.AddListener(() => _command.OnTriggered(new(calendarId)));
        }
    }

    public class OpenDetailedCalendarViewCommand : ActionListenerMono<SingleActionParam<IdOf<Calendar>>>
    {
        [Inject] private UiController _uiController;
        [Inject] private CalendarDetailedUiFactory _calendarDetailedUiFactory;
        public override void OnTriggered(SingleActionParam<IdOf<Calendar>> param)
        {
            _uiController.CloseUi(UiType.ListCalendar);
            _uiController.OpenUi(UiType.SingleCalendarDetailed, _calendarDetailedUiFactory.Create(param.Value));
        }
    }


}