#nullable enable
using Common.ButtonBase;
using Common.Model.Builder;
using Common.Triggers;
using Common.Triggers.GameController;
using Zenject;

namespace InGameUi.Button.Calendar_.Create
{
    public class CreateCalendarNameChangedActionTrigger : TextInputChangedActionTrigger<CreateCalendarNameChangedCommand>
    {
    }

    public class CreateCalendarNameChangedCommand : ActionListenerMono<SingleActionParam<string>>
    {
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder;
        public override void OnTriggered(SingleActionParam<string> param)
        {
            _calendarBuilderHolder.Builder.WithName(param.Value);
        }
    }
}