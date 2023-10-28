#nullable enable
using Common;
using Common.Model;
using Common.Model.Abstractions;
using Common.Utils;
using UnityEngine;

namespace Game.Common
{
    public abstract class ValueHolder<TValue> : MonoBehaviour
        where TValue : notnull
    {
        private Option<TValue> _value;

        public TValue Value
        {
            get => _value.Value;
            set => _value = value.ToOption();
        }

        private void Start()
        {
            _value.ExpectNotNull($"Value for holder of {typeof(TValue).FullName} was not provided on game object {gameObject.name}", gameObject);
        }
    }

    public abstract class ModelHolder<T> : ValueHolder<T> where T : IModel<T> { }
    
    public abstract class ModelIdHolder<T> : ValueHolder<IdOf<T>> where T : IModel<T> { }
}