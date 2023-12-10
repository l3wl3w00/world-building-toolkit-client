#nullable enable
using Common.ButtonBase;
using Common.Model.Builder;
using Common.Triggers;
using Common.Triggers.GameController;
using Common.Utils;
using InGameUi.Util;
using TMPro;
using Zenject;

namespace InGameUi.Button.Calendar_.Create
{
    public class CreateYearPhaseDaysChangedActionTrigger : UserControlledActionTrigger<TMP_InputField>
    {
        [Inject] private CreateYearPhaseDaysChangedCommand _command;
        protected override void RegisterListener(TMP_InputField component)
        {
            component.onValueChanged.AddListener(v =>
            {
                var param = new CreateYearPhaseDaysChangedParameters(v.ToUInt(), transform.GetParentDirectlyUnderContent().GetSelfIndexInParent());
                _command.OnTriggered(param);
            });
        }
    }
    
    public record CreateYearPhaseDaysChangedParameters(uint NewNumberOfDays, uint Index) : IActionParam;
    public class CreateYearPhaseDaysChangedCommand : ActionListenerMono<CreateYearPhaseDaysChangedParameters>
    {
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder;
        public override void OnTriggered(CreateYearPhaseDaysChangedParameters param)
        {
            var index = param.Index.ToInt();
            var yearPhases = _calendarBuilderHolder.Builder.YearPhases;
            yearPhases[index] = yearPhases[index] with
            {
                NumberOfDays = param.NewNumberOfDays
            };
        }
    }
}