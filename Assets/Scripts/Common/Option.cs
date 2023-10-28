#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Common
{
    public interface IOption
    {
        public bool HasValue { get; }
        public bool NoValue { get; }
        
        public object ValueAsObject { get; }
    }

    public readonly struct Option<T> : IOption where T : notnull
    {
        private readonly T? _value;
        
        private Option(T? value, bool hasValue)
        {
            HasValue = hasValue;
            _value = value;
        }

        public T Value
        {
            get
            {
                if (NoValue) throw new ValueNotFoundException();
                return _value!;
            }
        }

        public bool HasValue { get; }

        public bool NoValue => !HasValue;
        public object ValueAsObject => Value;
        public static Option<T> None => new(default, false);

        public static Option<T> Some(T value) => new(value, true);

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

        public E Map<E>(Func<T, E> onHasValue, Func<E> onNull) where E : notnull
        {
            if (HasValue) return onHasValue(Value);
            return onNull.Invoke();
        }
        
        public E Map<E>(Func<T, E> onHasValue, E valueOnNull) where E : notnull
        {
            if (HasValue) return onHasValue(Value);
            return valueOnNull;
        }

        public Option<E> NullOr<E>(Func<T, E> mapping) where E : notnull
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
        public T ExpectNotNull(string message, GameObject? context = null)
        {
            if (NoValue) Debug.LogError(message, context);
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

        public override string ToString()
        {
            if (NoValue) return "Option.None";
            return $"Some : {Value.ToString()}";
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
        public static Option<IOption> ExpectExactlyOneHasValue(this ICollection<IOption> options)
        {

            IOption? result = null;
            foreach (var option in options)
            {
                if (option.HasValue)
                {
                    if (result != null) return Option<IOption>.None;
                    result = option;
                }
            }
            if (result == null) return Option<IOption>.None;

            return result.ToOption();
        }
        
        public static Option<T> ToOption<T>(this T? t) where T : notnull
        {
            return Option<T>.FromNullable(t);
        }
        
        public static Option<T2> Cast<T1,T2>(this T1 t1) where T2 : class => (t1 as T2).ToOption();
    }
}