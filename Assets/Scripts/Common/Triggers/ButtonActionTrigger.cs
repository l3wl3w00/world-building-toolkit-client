#nullable enable
using Common.ButtonBase;
using Common.Triggers.GameController;
using Common.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Common.Triggers
{
    public class ButtonActionTrigger<TActionListener> : UserControlledActionTrigger<Button> where TActionListener : ActionListenerMono<NoActionParam>
    {
        [Inject] private TActionListener _actionListener;

        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _actionListener);
        }

        protected override void RegisterListener(Button component)
        {
            RegisterListener(component, _actionListener);
        }
        
        protected virtual void RegisterListener(Button component, TActionListener actionListener)
        {
            component.onClick.AddListener(() => actionListener.OnTriggered(new NoActionParam()));
        }
    }

    public abstract class ActionListener : ActionListenerMono<NoActionParam>
    {
        
    }

    public class GameObjectFactoryActionListener<TFactory> : ActionListener
        where TFactory : IFactory<GameObject>
    {
        [Inject] private TFactory _factory;


        public override void OnTriggered(NoActionParam param)
        {
            _factory.Create();
        }
    }  
}