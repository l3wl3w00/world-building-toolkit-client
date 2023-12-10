#nullable enable
using System;

namespace Common.Model.Abstractions
{
    public interface IModel { }

    public interface IModel<T> : IModel
        where T : IModel<T>
    {
        IdOf<T> Id { get; }
    }

    public interface IModelBuilder<out T> where T : IModel<T>
    {
        T Build();
    }
    
    public abstract class ModelBuilder<T> : IModelBuilder<T> where T : IModel<T>
    {
        public abstract T Build();
        protected virtual void BeforeBuild() { }
        protected virtual void OnConstruct() { }
    }
}