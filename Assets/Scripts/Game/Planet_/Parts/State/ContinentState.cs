#nullable enable
using System;
using Game.Continent_;
using Game.Region_;
using UnityEditor;
using UnityEngine;

namespace Game.Planet_.Parts.State
{
    public enum ContinentsState
    {
        SelectContinent,
        SelectedRegion,
        ContinentInCreation,
        RegionInCreation,
        EditPlanet,
        CreatePlanet,
        Null
    }

    public struct Unit
    {
    }

    public static class StateOperationUtils
    {
        public static void PrintExpectedStateMessage(Type expectedType, IContinentState actualState, string operation)
        {
            Debug.LogError($"State was expected to be {expectedType.Name} when doing operation: " +
                           $"[{operation}], but it was {actualState.GetType().Name}");
        }
    }


    public interface IContinentStateOperation<in TState, out TResult> where TState : IContinentState
    {
        TResult Apply(TState state);

        TResult OnStateNotT(IContinentState state);
    }


    public interface IContinentState
    {
        void ClickedOnContinent(ContinentMonoBehaviour continent, Camera mainCamera)
        {
            LogUndefined(nameof(ClickedOnContinent));
        }
        void ClickedOnRegion(RegionMonoBehaviour region, Camera camera)
        {
            LogUndefined(nameof(ClickedOnRegion));
        }
        void StartCreatingNewContinent()
        {
            LogUndefined(nameof(StartCreatingNewContinent));
        }
        void StartCreatingNewRegion()
        {
            LogUndefined(nameof(StartCreatingNewRegion));
        }

        void UpdateVisibleLines()
        {
            LogUndefined(nameof(UpdateVisibleLines));
        }

        void OnStart();

        ContinentsState State { get; }

        private void LogUndefined(string operationName)
        {
            Debug.LogError($"{operationName} is undefined on state {GetType().Name}");
        }

        TResult Interact<TState, TResult>(IContinentStateOperation<TState, TResult> operation)
            where TState : IContinentState
        {
            if (this is TState thisAsT)
            {
                return operation.Apply(thisAsT);
            }
            return operation.OnStateNotT(this);
        }
    }
    
    
    public class InvalidContinentStateException : Exception
    {
        public InvalidContinentStateException(IContinentState state)
        {
            
        }
    }
}

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit { }
}