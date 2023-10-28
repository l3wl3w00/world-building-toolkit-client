#nullable enable
using System;
using JetBrains.Annotations;

namespace Common.Model.Abstractions
{
    public static class IdHelper
    {
        public static IdOf<T> ToTypesafe<T>(this Guid guid) where T : IModel<T> => new(guid);
        
    }
    
    public interface IModelId {  }
    public readonly struct IdOf<T> : IModelId, IEquatable<IdOf<T>> where T : IModel<T>
    {
        public readonly Guid Value;

        public override bool Equals(object? obj)
        {
            return obj is IdOf<Continent> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public IdOf(Guid value)
        {
            Value = value;
        }

        public bool Equals(IdOf<T> other)
        {
            return Value == other.Value;
        }

        public static bool operator ==(IdOf<T> a, IdOf<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(IdOf<T> a, IdOf<T> b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            return $"ContinentId({Value.ToString()})";
        }
    };
}