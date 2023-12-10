#nullable enable
using System.Linq;
using System.Text;
using Common.ButtonBase;
using Common.Model.Builder;
using Common.Triggers;
using Common.Triggers.GameController;
using Common.Utils;
using Zenject;

namespace InGameUi.Button.Calendar_.Create
{
    public class CreateCalendarFirstYearChangedActionTrigger : TextInputChangedActionTrigger<CreateCalendarFirstYearChangedCommand>
    {
        
    }
    
    public class CreateCalendarFirstYearChangedCommand : ActionListenerMono<SingleActionParam<string>>
    {
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder;
        public override void OnTriggered(SingleActionParam<string> param)
        {
            _calendarBuilderHolder.Builder.WithFirstYear(param.Value.ToULong());
        }
    }
}