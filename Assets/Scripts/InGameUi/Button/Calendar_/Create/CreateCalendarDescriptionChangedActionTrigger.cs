#nullable enable
using Common.ButtonBase;
using Common.Model.Builder;
using Common.Triggers;
using Common.Triggers.GameController;
using Zenject;

namespace InGameUi.Button.Calendar_.Create
{
    public class CreateCalendarDescriptionChangedActionTrigger : TextInputChangedActionTrigger<CreateCalendarDescriptionChangedCommand>
    {
    }
    
        
    public class CreateCalendarDescriptionChangedCommand : ActionListenerMono<SingleActionParam<string>>
    {
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder;
        public override void OnTriggered(SingleActionParam<string> param)
        {
            _calendarBuilderHolder.Builder.WithDescription(param.Value);
        }
    }
}