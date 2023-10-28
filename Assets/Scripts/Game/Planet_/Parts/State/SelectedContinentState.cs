#nullable enable
using Game.Common;
using Game.Continent_;
using Game.Region_;
using UnityEngine;

namespace Game.Planet_.Parts.State
{
    public record SelectedContinentState(
        ContinentMonoBehaviour SelectedContinent,
        PlanetMonoBehaviour PlanetMono) : IContinentState
    {
        public void ClickedOnContinent(ContinentMonoBehaviour continent, Camera mainCamera)
        {
            if (SelectedContinent == continent)
            {
                PlanetMono.ToEditPlanetState();
                return;
            }
            PlanetMono.SelectContinent(continent);
        }

        public void ClickedOnRegion(RegionMonoBehaviour region, Camera camera)
        {
            PlanetMono.SelectRegion(region);
        }

        public void StartCreatingNewContinent()
        {
            var parentId = SelectedContinent.Continent.Id;
            var builder = ContinentMonoBuilder.Create(new(parentId, PlanetMono, PlanetMono));
            PlanetMono.SetContinentState(new ContinentInCreationState(builder, PlanetMono));
        }

        public void StartCreatingNewRegion()
        {
            var continentId = SelectedContinent.Continent.Id;
            var builder = RegionMonoBuilder.Create(new(continentId, PlanetMono, PlanetMono));
            PlanetMono.SetContinentState(new RegionInCreationState(builder, PlanetMono));
        }

        public void OnStart()
        {
            PlanetMono.MakeAllLinesInvisibleExcept(SelectedContinent);
        }

        public ContinentsState State => ContinentsState.SelectContinent;
        public void UpdateVisibleLines() => SelectedContinent.UpdateLines();
    }
}