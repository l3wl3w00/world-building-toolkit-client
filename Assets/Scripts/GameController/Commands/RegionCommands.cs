#nullable enable
using Common.ButtonBase;
using Common.Model;
using Game.Planet_.Parts.State;
using Game.Region_;
using UnityEngine;

namespace GameController.Commands
{
    public class ChangeRegionColorCommand : StateDependantCommand<SingleActionParam<Color>,SelectedRegionState>
    {
        protected override Unit Apply(SelectedRegionState state, SingleActionParam<Color> param)
        {
            state.RegionMono.OnColorChanged(param.Value);
            return default;
        }
    }
    
    public class StartCreatingRegionCommand : Command
    {
        public override void OnTriggered(NoActionParam param)
        {
            PlanetMono.StartCreatingNewRegion();
        }
    }

    public class CancelCreatingRegionCommand : StateDependantCommand<RegionInCreationState>
    {
        protected override Unit Apply(RegionInCreationState state, NoActionParam param)
        {
            Destroy(state.Builder);
            PlanetMono.ToEditPlanetState();
            return new Unit();
        }
    }
    
    public class CreateRegionCommand : StateDependantCommand<SingleActionParam<Region>,RegionInCreationState>
    {
        protected override Unit Apply(RegionInCreationState state, SingleActionParam<Region> param)
        {
            var region = param.Value;
            state.Builder.Build(region);
            state.PlanetMono.UpdatePlanetMeshes();
            state.PlanetMono.ToEditPlanetState();
            Destroy(state.Builder.gameObject);
            return default;
        }
    }
    
}