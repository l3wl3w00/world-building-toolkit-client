#nullable enable
using Common.ButtonBase;
using Common.Triggers.GameController;
using Common.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Common.Triggers
{
    public class ToggleActionTrigger<TActionListener> : UserControlledActionTrigger<Toggle>
        where TActionListener : IActionListener<SingleActionParam<bool>>
    {
        [Inject] private TActionListener _actionListener = default!; // Asserted in OnStart
        
        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _actionListener);
        }
        protected override void RegisterListener(Toggle component)
        {
            component.onValueChanged.AddListener(v => _actionListener.OnTriggered(new(v)));
        }
    }
}