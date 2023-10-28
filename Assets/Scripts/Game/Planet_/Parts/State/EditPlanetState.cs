#nullable enable
using Game.Continent_;
using Game.Region_;
using UnityEngine;

namespace Game.Planet_.Parts.State
{
    public record EditPlanetState(PlanetMonoBehaviour PlanetMono) : IContinentState
    {
        public void ClickedOnContinent(ContinentMonoBehaviour continent, Camera mainCamera) =>
            PlanetMono.SelectContinent(continent);
        
        public void ClickedOnRegion(RegionMonoBehaviour region, Camera mainCamera) =>
            PlanetMono.SelectRegion(region);
        public void OnStart() => PlanetMono.MakeAllLinesInvisible();
        public ContinentsState State => ContinentsState.EditPlanet;
        public void UpdateVisibleLines()
        {
            
        }
        //
        // public IContinentState NextState(IContinentStateChangeEvent stateChangeEvent)
        // {
        //     if (stateChangeEvent is IContinentStateChangeEventOnEditPlanetState e)
        //         return e.GetNextState(this);
        //     return this;
        // }
    }
    
    // public interface IContinentStateChangeEventOnEditPlanetState : IContinentStateChangeEvent
    // {
    //     IContinentState GetNextState(EditPlanetState editPlanetState);
    // }
}