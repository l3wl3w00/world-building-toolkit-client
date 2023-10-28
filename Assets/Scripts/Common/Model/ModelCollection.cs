#nullable enable
using System.Collections;
using System.Collections.Generic;
using Common.Model.Abstractions;
using Common.Utils;
using UnityEngine;
using Zenject;

namespace Common.Model
{
    public class ModelCollection<T> : IEnumerable<T>
        where T : IModel<T>
    {
        private readonly ICollection<T> _ts = new HashSet<T>();

        public Option<T> GetById(IdOf<T> id)
        {
            foreach (var t in _ts)
            {
                if (t.Id == id) return t;
            }

            return Option<T>.None;
        }
        
        public ModelCollection<T> Add(T t)
        {
            _ts.Add(t);
            return this;
        }
        
        public ModelCollection<T> Add(IEnumerable<T> ts)
        {
            ts.ForEach(_ts.Add);
            return this;
        }
        public T GetByIdNotNull(IdOf<T> id) => 
            GetById(id).ExpectNotNull($"Continent with id {id} was not found");

        public IEnumerator<T> GetEnumerator() => _ts.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class BuilderHolder<T, TBuilder> 
        where T : IModel<T>
        where TBuilder : IModelBuilder<T>, new()
    {
        public Option<TBuilder> Builder { get; private set; } = Option<TBuilder>.None;

        public void StartBuildingModel() 
        {
            
            Builder = Option<TBuilder>.Some(new TBuilder());
        }
    }

    public class ModelCollectionQuery<T> : MonoBehaviour, IQuery<ModelCollection<T> > where T : IModel<T>
    {
        [Inject] private ModelCollection<T> _modelCollection;
        public ModelCollection<T> Get() => _modelCollection;
    }

    public class CalendarModelCollectionQuery : ModelCollectionQuery<Calendar> { }
}