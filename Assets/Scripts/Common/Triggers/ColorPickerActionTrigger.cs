#nullable enable
using Common.ButtonBase;
using Common.Triggers.GameController;
using UnityEngine;
using Zenject;

namespace Common.Triggers
{
    public class ColorActionTrigger<TActionListener> : UserControlledActionTrigger<FlexibleColorPicker>
        where TActionListener : ActionListenerMono<SingleActionParam<Color>>
    {
        [Inject] protected TActionListener _actionListener;

        protected override void RegisterListener(FlexibleColorPicker component)
        {
            component.onColorChange.AddListener(v => _actionListener.OnTriggered(new SingleActionParam<Color>(v)));
        }
    }
}