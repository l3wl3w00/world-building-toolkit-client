#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Model.Abstractions;
using Common.Utils;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Common.Model
{
    public class ModelCollection<T> : IEnumerable<T>
        where T : IModel<T>
    {
        private readonly Dictionary<IdOf<T>, T> _ts = new();
        public int Count => _ts.Count;

        public Option<T> GetById(IdOf<T> id) => _ts.GetValueOrDefault(id).ToOption();

        public Option<T> RemoveById(IdOf<T> id)
        {
            var itemToRemoveOpt = GetById(id);
            if (itemToRemoveOpt.NoValueOut(out var itemToRemove)) return Option<T>.None;
            
            _ts.Remove(id);
            return itemToRemove.ToOption();
        }
        public Ref<T> Add(T t)
        {
            _ts.Add(t.Id,t);
            return t.Id.ToRef(this);
        }
        
        public ModelCollection<T> Add(IEnumerable<T> ts)
        {
            ts.ForEach(t => Add(t));
            return this;
        }
        public T GetByIdNotNull(IdOf<T> id) => 
            GetById(id).ExpectNotNull($"Continent with id {id} was not found");

        public IEnumerator<T> GetEnumerator() => _ts.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ReplaceIfExists(IdOf<T> id, Func<T, T> func)
        {
            if (!_ts.TryGetValue(id, out var t)) return false;
            
            _ts[id] = func(t);
            return true;
        }
    }

    public class BuilderHolder<T, TBuilder> 
        where T : IModel<T>
        where TBuilder : IModelBuilder<T>, new()
    {
        public Option<TBuilder> BuilderOpt { get; private set; } = Option<TBuilder>.None;
        public bool IsModelInCreation => BuilderOpt.HasValue;
        public bool IsModelNotInCreation => BuilderOpt.NoValue;

        public TBuilder Builder =>
            BuilderOpt.ExpectNotNull($"{typeof(T).Name} was not in creation even though it was expected to be!");

        public TBuilder StartBuildingModel() 
        {
            if (BuilderOpt.HasValue)
            {
                Debug.LogWarning($"Tried to start creating {typeof(T)}, but one was already in creation");
            }
            BuilderOpt = Option<TBuilder>.Some(new TBuilder());
            return Builder;
        }
        
        public void DoIfBuilderPresent(Action<TBuilder> action)
        {
            BuilderOpt.DoIfNotNull(action);
        }
        
        public UnityAction<T2> CreateAction<T2>(Action<TBuilder, T2> action)
        {
            return t => BuilderOpt.DoIfNotNull(b => action(b, t));
        }
        
        public T BuildAndReset()
        {
            var result = Builder.Build();
            BuilderOpt = Option<TBuilder>.None;
            return result;
        }
        
        public void CancelCreation()
        {
            BuilderOpt = Option<TBuilder>.None;
        }
    }

    public class ModelCollectionQuery<T> : MonoBehaviour, IQuery<ModelCollection<T> > where T : IModel<T>
    {
        [Inject] private ModelCollection<T> _modelCollection = null!; // Asserted in Start 

        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _modelCollection);
        }

        public ModelCollection<T> Get() => _modelCollection;
    }

    public class CalendarModelCollectionQuery : ModelCollectionQuery<Calendar> { }
}