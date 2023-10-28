#nullable enable
using System;
using Codice.Client.BaseCommands.Merge;
using Common.ButtonBase;
using Common.Triggers.GameController;
using Common.Utils;
using Game.Planet_.Parts;
using Game.Planet_.Parts.State;
using UnityEngine;
using Zenject;

namespace GameController
{
    public abstract class Command<TParams> : ActionListenerMono<TParams>
        where TParams : IActionParam
    {
        [Inject] private PlanetMonoBehaviour _planetMonoBehaviour = null!; // Asserted in Start
        protected PlanetMonoBehaviour PlanetMono => _planetMonoBehaviour;
        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _planetMonoBehaviour);
        }
    }

    public abstract class Command : Command<NoActionParam> { }

    public abstract class StateDependantCommand<TParams, TState, TResult> : ActionListenerMono<TParams>
        where TParams : IActionParam
        where TState : IContinentState
    {
        [Inject] private PlanetMonoBehaviour _planetMonoBehaviour = null!; // Asserted in Start
        protected PlanetMonoBehaviour PlanetMono => _planetMonoBehaviour;
        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _planetMonoBehaviour);
        }

        public override void OnTriggered(TParams param)
        {
            PlanetMono.ApplyStateOperation(new StateOperation(this, param));
        }
        protected abstract TResult Apply(TState state, TParams param);

        protected virtual TResult OnStateNotT(IContinentState state, TParams param)
        {
            StateOperationUtils.PrintExpectedStateMessage(typeof(TState), state, GetType().Name);
            return default;
        }
        
        
        private record StateOperation(StateDependantCommand<TParams, TState, TResult> command, TParams param) : IContinentStateOperation<TState,TResult>
        {
            public TResult Apply(TState state)
            {
                return command.Apply(state, param);
            }

            public TResult OnStateNotT(IContinentState state)
            {
                return command.OnStateNotT(state, param);
            }
        }
    }

    public abstract class StateDependantCommand<TState> : StateDependantCommand<NoActionParam, TState, Unit> 
        where TState : IContinentState { }
    public abstract class StateDependantCommand<TParam, TState> : StateDependantCommand<TParam, TState, Unit>
        where TState : IContinentState 
        where TParam : IActionParam
    { }
}