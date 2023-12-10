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
    public class CreateYearPhaseNameChangedActionTrigger : UserControlledActionTrigger<TMP_InputField>
    {
        [Inject] private CreateYearPhaseNameChangedCommand _command;
        protected override void RegisterListener(TMP_InputField component)
        {
            component.onValueChanged.AddListener(v =>
            {
                var param = new CreateYearPhaseNameChangedParameters(v, transform.GetParentDirectlyUnderContent().GetSelfIndexInParent());
                _command.OnTriggered(param);
            });
        }
    }

    public record CreateYearPhaseNameChangedParameters(string NewName, uint Index) : IActionParam;
    public class CreateYearPhaseNameChangedCommand : ActionListenerMono<CreateYearPhaseNameChangedParameters>
    {
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder;
        public override void OnTriggered(CreateYearPhaseNameChangedParameters param)
        {
            var index = (int) param.Index;
            var yearPhases = _calendarBuilderHolder.Builder.YearPhases;
            yearPhases[index] = yearPhases[index] with
            {
                Name = param.NewName
            };
        }
    }

    
}

namespace System.Runtime.CompilerServices { public class IsExternalInit { } }