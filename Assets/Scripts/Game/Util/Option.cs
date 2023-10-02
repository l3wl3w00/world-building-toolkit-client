#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Util
{
    public abstract class Option<T> : IEnumerable<T>
    {
        public abstract T Value { get; }

        public T? NullableValue
        {
            get
            {
                if (HasValue) return Value;
                return default;
            }
        }

        public abstract bool HasValue { get; }
        public bool NoValue => !HasValue;
        public static Option<T> None => new None<T>();

        public IEnumerator<T> GetEnumerator()
        {
            if (HasValue) yield return Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator Option<T>(T? t)
        {
            return FromNullable(t);
        }

        public static Option<T> FromNullable(T? value)
        {
            if (value == null) return None;
            return Some(value);
        }

        public static Option<T> Some(T value)
        {
            return new Some<T>(value);
        }

        public T MapIfNull<E>(Func<E> func) where E : T
        {
            if (HasValue) return Value;
            return func();
        }

        public E Map<E>(Func<T, E> onHasValue, Func<E>? onNull)
        {
            if (HasValue) return onHasValue(Value);
            return onNull();
        }

        public Option<E> MapIfNotNull<E>(Func<T, E> mapping)
        {
            if (HasValue) return mapping(Value).ToOption();
            return Option<E>.None;
        }


        public Option<T> DoIfNotNull(Action<T> action)
        {
            if (HasValue) action(Value);
            return this;
        }

        public Option<T> DoIfNull(Action action)
        {
            if (NoValue) action();
            return this;
        }
    }

    internal class Some<T> : Option<T>
    {
        public override T Value { get; }

        public override bool HasValue => true;

        public Some(T value)
        {
            Value = value;
        }
    }

    internal class None<T> : Option<T>
    {
        public override T Value => throw new ValueNotFoundException();
        public override bool HasValue => false;
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
    }
}