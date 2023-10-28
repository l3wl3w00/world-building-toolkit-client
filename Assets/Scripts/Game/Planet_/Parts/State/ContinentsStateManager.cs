#nullable enable
using System.Collections.Generic;

namespace Game.Planet_.Parts.State
{
    // public interface IContinentStateChangeEvent
    // {
    //     public IContinentState CreateNext(PlanetMonoBehaviour planetMono);
    // }
    //
    //
    //
    // public record PlanetCreated : IContinentStateChangeEvent
    // {
    //     public IContinentState CreateNext(PlanetMonoBehaviour planetMono) => new EditPlanetState(planetMono);
    // }
    // public record ContinentChildCreationStarted(ContinentMonoBuilder Builder) : IContinentStateChangeEvent
    // {
    //     public IContinentState CreateNext(PlanetMonoBehaviour planetMono) => new ContinentInCreationState(Builder, planetMono);
    // }
    // public record ContinentChildCreationStopped(ContinentMonoBuilder Builder) : IContinentStateChangeEvent
    // {
    //     public IContinentState CreateNext(PlanetMonoBehaviour planetMono) => new EditPlanetState(planetMono);
    // }
    // public record ContinentClicked()
    //
    // public record Continent : IContinentStateChangeEvent;
    //
    // public class ContinentsStateManager
    // {
    //     private readonly PlanetMonoBehaviour _planetMonoBehaviour;
    //     private IContinentState _currentState;
    //
    //     public ContinentsStateManager(PlanetMonoBehaviour planetMonoBehaviour)
    //     {
    //         _planetMonoBehaviour = planetMonoBehaviour;
    //     }
    //
    //     public void ApplyEvent(IContinentStateChangeEvent continentsStateChangeEvent)
    //     {
    //         _currentState = continentsStateChangeEvent.CreateNext(_planetMonoBehaviour);
    //     }
    // }
}