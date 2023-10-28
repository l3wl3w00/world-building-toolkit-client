#nullable enable
using System;
using Common;
using Common.ButtonBase;
using Common.Utils;
using Game.Planet_.Parts;
using Game.Planet_.Parts.State;
using UnityEngine;
using Zenject;

namespace GameController.Queries
{

    public abstract class StateDependantQuery<TResult, TState> : MonoBehaviour, IQuery<TResult>, IContinentStateOperation<TState, TResult>
        where TState : IContinentState 
        where TResult : notnull
    {
        
        private PlanetMonoBehaviour _planetMonoBehaviour = null!; // Asserted in Start

        [Inject]
        public void Construct(PlanetMonoBehaviour planetMono)
        {
            _planetMonoBehaviour = planetMono;
        }
        protected PlanetMonoBehaviour PlanetMono => _planetMonoBehaviour;
        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _planetMonoBehaviour); 
        }
        public abstract TResult Apply(TState state);

        public TResult Get() => PlanetMono.ApplyStateOperation(this);

        public TResult OnStateNotT(IContinentState state)
        {
            StateOperationUtils.PrintExpectedStateMessage(typeof(TState), state, $"query {GetType().Name}");
            throw new InvalidContinentStateException(state);
        }
    }
}