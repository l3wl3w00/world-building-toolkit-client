#nullable enable
using System;
using System.Linq;
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
            return obj is IdOf<T> other && Equals(other);
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
            return $"{typeof(T).Name}Id({Value.ToString()})";
        }

        public Ref<T> ToRef(ModelCollection<T> models) => new(this, models);
    };

    public static class IdUtils
    {
        public static T GetModel<T>(this IdOf<T> id, ModelCollection<T> models) where T : IModel<T> =>
            models.GetById(id)
                .ExpectNotNull($"No calendar was found with id {id}");

        public static Option<T> GetModelOpt<T>(this IdOf<T> id, ModelCollection<T> models) where T : IModel<T> =>
            models.GetById(id);
    }
    
    public readonly struct Ref<T> 
        where T : IModel<T>
    {
        public readonly IdOf<T> Id;
        public readonly ModelCollection<T> Models;

        public Ref(IdOf<T> id, ModelCollection<T> models)
        {
            Id = id;
            Models = models;
        }

        public T Get() => GetOpt().ExpectNotNull($"No {typeof(T).Name} was found with id '{Id}'");
        public Option<T> GetOpt() => Models.GetById(Id);
        public void Update(Func<T, T> updateFunc) => Models.ReplaceIfExists(Id, updateFunc);
        public void Delete() => Models.RemoveById(Id);
    }
}