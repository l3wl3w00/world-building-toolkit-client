#nullable enable
using Common.ButtonBase;
using Common.Triggers.GameController;
using Common.Utils;
using UnityEngine;
using Zenject;

namespace Common.Triggers
{
    public class ColorActionTrigger<TActionListener> : UserControlledActionTrigger<FlexibleColorPicker>
        where TActionListener : ActionListenerMono<SingleActionParam<Color>>
    {
        [Inject] protected TActionListener _actionListener = default!; // Asserted in OnStart

        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _actionListener);
        }
        
        protected override void RegisterListener(FlexibleColorPicker component)
        {
            component.onColorChange.AddListener(v => _actionListener.OnTriggered(new SingleActionParam<Color>(v)));
        }
    }
}