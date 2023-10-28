#nullable enable
using Game.Continent_;
using Game.Region_;
using UnityEngine;

namespace Game.Planet_.Parts.State
{
    public record SelectedRegionState(RegionMonoBehaviour RegionMono, PlanetMonoBehaviour PlanetMono) : IContinentState
    {
        public void ClickedOnContinent(ContinentMonoBehaviour continent, Camera mainCamera)
        {
            PlanetMono.SelectContinent(continent);
        }

        public void ClickedOnRegion(RegionMonoBehaviour region, Camera camera)
        {
            if (region == RegionMono)
            {
                PlanetMono.ToEditPlanetState();
                return;
            }
            PlanetMono.SelectRegion(region);
        }

        public void UpdateVisibleLines()
        {
            RegionMono.UpdateLines();
        }

        public void OnStart()
        {
            PlanetMono.MakeAllLinesInvisibleExcept(RegionMono);
        }

        public ContinentsState State => ContinentsState.SelectedRegion;
    }
}