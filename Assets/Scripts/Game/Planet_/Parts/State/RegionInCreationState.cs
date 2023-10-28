#nullable enable
using System.Linq;
using Common;
using Game.Continent_;
using Game.Region_;
using UnityEngine;

namespace Game.Planet_.Parts.State
{
    public record RegionInCreationState(RegionMonoBuilder Builder, PlanetMonoBehaviour PlanetMono): IContinentState
    {
        public void ClickedOnContinent(ContinentMonoBehaviour continent, Camera mainCamera)
        {
            if (continent.Continent.Id != Builder.ContinentId)
            {
                Debug.LogWarning("Clicked on a continent that is not the parent of the region in creation!");
                return;
            }
            
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastAll(ray);

            if (hits.Length is 0) return;
            var hit = hits.FirstOrDefault(hit => 
                    hit.collider.GetComponent<ContinentMonoBehaviour>() == continent)
                .ToOption()
                .ExpectNotNull($"ray hit did not contain a game object with {nameof(ContinentMonoBehaviour)}, " +
                               $"even though {nameof(ClickedOnContinent)} was called on {GetType().Name}");

            var leftClick = Input.GetMouseButtonDown(0);
            if (!leftClick) return;

            Builder.AddControlPoint(hit.point);
        }

        public void ClickedOnRegion(RegionMonoBehaviour region, Camera camera)
        {
            var continent = PlanetMono.FindContinent(region.Region.ContinentId);
            ClickedOnContinent(continent, camera);
        }

        public void OnStart()
        {
            PlanetMono.MakeAllLinesInvisibleExcept(Builder);
        }

        public void UpdateVisibleLines() => Builder.UpdateLines();
        public ContinentsState State => ContinentsState.RegionInCreation;
    }
}