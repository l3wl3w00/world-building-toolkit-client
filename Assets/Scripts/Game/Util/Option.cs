#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Game.Util
{
    public readonly struct Option<T>
    {
        private Option(T? value)
        {
            NullableValue = value;
        }

        public T Value
        {
            get
            {
                if (NullableValue == null) throw new ValueNotFoundException();
                return NullableValue;
            }
        }

        public T? NullableValue { get; }

        public bool HasValue => NullableValue != null;
        public bool NoValue => !HasValue;
        public static Option<T> None => new(default);

        public static Option<T> Some(T value) => new(value);

        public static implicit operator Option<T>(T? t)
        {
            return FromNullable(t);
        }

        public static Option<T> FromNullable(T? value)
        {
            if (value == null) return None;
            return Some(value);
        }

        public T ValueOr(T t)
        {
            if (HasValue) return Value;
            return t;
        }

        public E Map<E>(Func<T, E> onHasValue, Func<E> onNull)
        {
            if (HasValue) return onHasValue(Value);
            return onNull.Invoke();
        }
        
        public E Map<E>(Func<T, E> onHasValue, E valueOnNull)
        {
            if (HasValue) return onHasValue(Value);
            return valueOnNull;
        }

        public Option<E> NoneOr<E>(Func<T, E> mapping)
        {
            if (HasValue) return mapping(Value).ToOption();
            return Option<E>.None;
        }


        public Option<T> DoIfNotNull(Action<T> action)
        {
            if (HasValue) action(Value);
            return this;
        }
        
        public async Task<Option<T>> DoIfNotNullAsync(Func<T, Task> action)
        {
            if (HasValue) await action(Value);
            return this;
        }

        public Option<T> DoIfNull(Action action)
        {
            if (NoValue) action();
            return this;
        }
        
        public Option<T> LogErrorIfNull(string message)
        {
            if (NoValue) Debug.LogError(message);
            return this;
        }
        
        public Option<T> LogWarnIfNull(string message)
        {
            if (NoValue) Debug.LogWarning(message);
            return this;
        }
        public T ExpectNotNull(string message)
        {
            if (NoValue) Debug.LogError(message);
            return Value;
        }
        
        public T ExpectNotNull(string variableName, Delegate methodOrProperty)
        {
            var methodInfo = methodOrProperty.Method;
            var methodName = methodInfo.Name;
            var type = methodInfo.DeclaringType;
            
            var message = new StringBuilder($"variable '{variableName}' in method or property '{methodName}'");
            if (type != null) message.Append($" of class '{type.Name}'");
            message.Append("was null, even though it was expected not to be");
            return ExpectNotNull(message.ToString());
        }
    }
    public class ValueNotFoundException : Exception
    {
        public ValueNotFoundException() : base("Getting the value of optional failed, because value was none")
        {
        }
    }

    public static class OptionsExtension
    {
        public static Option<T> ToOption<T>(this T? t)
        {
            return Option<T>.FromNullable(t);
        }
        
        public static Option<T2> Cast<T1,T2>(this T1 t1) where T2 : class => (t1 as T2).ToOption();
    }
}