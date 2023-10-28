#nullable enable
using Common.ButtonBase;
using Common.Triggers.GameController;
using TMPro;
using Zenject;

namespace Common.Triggers
{
    public class TextInputChangedActionTrigger<TActionListener> : UserControlledActionTrigger<TMP_InputField>
        where TActionListener : ActionListenerMono<SingleActionParam<string>>
    {
        [Inject] private TActionListener _actionListener;

        protected override void RegisterListener(TMP_InputField component)
        {
            component.onValueChanged.AddListener(v => _actionListener.OnTriggered(new(v)));
        }
    }
}