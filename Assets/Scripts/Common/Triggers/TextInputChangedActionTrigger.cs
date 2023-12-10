#nullable enable
using Common.ButtonBase;
using Common.Triggers.GameController;
using Common.Utils;
using TMPro;
using Zenject;

namespace Common.Triggers
{
    public class TextInputChangedActionTrigger<TActionListener> : UserControlledActionTrigger<TMP_InputField>
        where TActionListener : ActionListenerMono<SingleActionParam<string>>
    {
        [Inject] private TActionListener _actionListener = default!; // Asserted in OnStart

        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _actionListener);
        }

        protected override void RegisterListener(TMP_InputField component)
        {
            component.onValueChanged.AddListener(v => _actionListener.OnTriggered(new(v)));
        }
    }
}