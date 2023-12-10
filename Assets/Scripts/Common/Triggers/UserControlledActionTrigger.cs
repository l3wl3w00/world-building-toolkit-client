#nullable enable
using Common.Utils;
using UnityEngine;

namespace Common.Triggers
{
    public abstract class UserControlledActionTrigger<TComponent> 
        : MonoBehaviour where TComponent : Component
    {
        protected void Start()
        {
            var component = GetComponent<TComponent>()
                .ToOption()
                .ExpectNotNull($"{GetType().Name} applied to {gameObject.GetPath()}, which has no {typeof(TComponent).Name} Component");
            RegisterListener(component);
            OnStart();
        }

        protected abstract void RegisterListener(TComponent component);
        protected virtual void OnStart()
        {
        }
    }
}