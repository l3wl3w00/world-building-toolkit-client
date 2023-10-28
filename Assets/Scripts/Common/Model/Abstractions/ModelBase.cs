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
}