#nullable enable
using Common;
using Common.Model;
using Common.Model.Abstractions;
using Common.Utils;
using UnityEngine;
using Zenject;

namespace Game.Common
{
    public abstract class ValueHolder<TValue> : MonoBehaviour
        where TValue : notnull
    {
        public Option<TValue> ValueOpt;

        public TValue Value
        {
            get => ValueOpt
                .ExpectNotNull($"Value for holder of {typeof(TValue).FullName} was not provided on game object {gameObject.name} when querying it");
            set => ValueOpt = value.ToOption();
        }
        
        public bool HasValueOut(out TValue? value)
        {
            return ValueOpt.HasValueOut(out value);
        }

        private void Start()
        {
            ValueOpt.ExpectNotNull($"Value for holder of {typeof(TValue).FullName} was not provided on game object {gameObject.name}", gameObject);
        }
    }

    public abstract class ModelHolder<T> : ValueHolder<T> where T : IModel<T> { }

    public abstract class ModelIdHolder<T> : ValueHolder<IdOf<T>> where T : IModel<T>
    {
    }

    public abstract class ModelRefHolder<T> : ValueHolder<Ref<T>>
        where T : IModel<T>
    {
    }

    public static class ModelIdHolderUtils
    {
        public static IdOf<T> GetIdOf<T>(this GameObject gameObject) where T : IModel<T> => 
            gameObject.GetModelIdHolder<T>().Value;
        
        public static IdOf<T> GetIdInParent<T>(this GameObject gameObject) where T : IModel<T> => 
            gameObject.GetModelIdHolderInParent<T>().Value;
        
        public static ModelIdHolder<T> GetModelIdHolder<T>(this GameObject gameObject)
            where T : IModel<T> =>
            gameObject.GetComponent<ModelIdHolder<T>>()
                .ToOption()
                .ExpectNotNull($"{gameObject.name} does not have a {typeof(ModelIdHolder<T>).FullName} component on it");
        
        public static ModelIdHolder<T> GetModelIdHolderInParent<T>(this GameObject gameObject)
            where T : IModel<T> =>
            gameObject
                .GetComponentInParent<ModelIdHolder<T>>()
                .ToOption()
                .ExpectNotNull($"{gameObject.name} does not have a {typeof(ModelIdHolder<T>).FullName} component in any of its parents, but is expected to");
    }

}