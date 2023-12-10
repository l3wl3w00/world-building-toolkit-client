#nullable enable
using System;
using Common.ButtonBase;
using Common.Triggers.GameController;
using Common.Utils;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Common.Triggers
{
    public class ButtonActionTrigger<TActionListener> : UserControlledActionTrigger<Button> where TActionListener : ActionListenerMono<NoActionParam>
    {
        [Inject] private TActionListener _actionListener = default!; // Asserted in OnStart

        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _actionListener);
        }

        protected sealed override void RegisterListener(Button component)
        {
            RegisterListener(component, _actionListener);
        }
        
        protected virtual void RegisterListener(Button component, TActionListener actionListener)
        {
            component.onClick.AddListener(() => actionListener.OnTriggered(new NoActionParam()));
        }
    }
    
    public abstract class ButtonActionTrigger : UserControlledActionTrigger<Button>
    {
        protected override void RegisterListener(Button component)
        {
            component.onClick.AddListener(GetClickListener());
        }

        protected abstract UnityAction GetClickListener();
    }

    public abstract class ActionListener : ActionListenerMono<NoActionParam>
    {
        
    }
}