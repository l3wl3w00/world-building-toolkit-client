#nullable enable
using Game.Continent_;
using UnityEngine;

namespace Game.Planet_.Parts.State
{
    public record PlanetInCreationState : IContinentState
    {
        public void ClickedOnContinent(ContinentMonoBehaviour continent, Camera mainCamera)
        {
            Debug.LogError($"Clicked on a continent while in state {nameof(PlanetInCreationState)}. This should not happen");
        }

        public void StartCreatingNewContinent()
        {
            Debug.LogError($"Started creating a new continent while in state {nameof(PlanetInCreationState)}. This should not happen");
        }

        public void OnStart() { }
        public ContinentsState State => ContinentsState.CreatePlanet;

        public void UpdateVisibleLines()
        {
            
        }
    }
}