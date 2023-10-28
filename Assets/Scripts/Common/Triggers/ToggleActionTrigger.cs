#nullable enable
using Common.ButtonBase;
using Common.Triggers.GameController;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Common.Triggers
{
    public class ToggleActionTrigger<TActionListener> : UserControlledActionTrigger<Toggle>
        where TActionListener : IControlActionListener<SingleActionParam<bool>>
    {
        [Inject] private TActionListener _actionListener;

        protected override void RegisterListener(Toggle component)
        {
            component.onValueChanged.AddListener(v => _actionListener.OnTriggered(new(v)));
        }
    }
}