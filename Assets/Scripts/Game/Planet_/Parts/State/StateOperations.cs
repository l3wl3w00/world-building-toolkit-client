#nullable enable
namespace Game.Planet_.Parts.State
{
    public interface IContinentStateCommand<in TState, in TParams> : IContinentStateOperation<TState, Unit>
        where TState : IContinentState
    {
        void Execute(TState state, TParams parameters);

        void OnStateNotT(IContinentState state, TParams parameters);
    }
    
        
    public interface IContinentStateCommand<in TState>
        where TState : IContinentState
    {
        void Execute(TState state);

        void OnStateNotT(IContinentState state);
    }

    public interface IContinentStateQuery<in TState, out TResult, in TParams>
        where TState : IContinentState
    {
        TResult Get(TState state, TParams parameters);

        TResult OnStateNotT(IContinentState state, TParams parameters);
    }
    
    public interface IContinentStateQuery<in TState, out TResult>
        where TState : IContinentState
    {
        TResult Get(TState state);

        TResult OnStateNotT(IContinentState state);
    }
}