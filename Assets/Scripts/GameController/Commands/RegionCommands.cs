#nullable enable
using Common.ButtonBase;
using Common.Model;
using Game.Planet_.Parts.State;
using Game.Region_;
using UnityEditor.Graphs;
using UnityEngine;
using Zenject;

namespace GameController.Commands
{
    public abstract class ChangeSelectedRegionModelCommand<TValue>
        : StateDependantCommand<SingleActionParam<TValue>, SelectedRegionState>
    {
        protected sealed override Unit Apply(SelectedRegionState state, SingleActionParam<TValue> param)
        {
            state.RegionMono.RegionRef.Update(r => UpdateRegion(r, param.Value));
            return default;
        }

        protected abstract Region UpdateRegion(Region region, TValue paramValue);
    }
    public class ChangeRegionColorCommand : ChangeSelectedRegionModelCommand<Color>
    {
        protected override Region UpdateRegion(Region region, Color color) => region with { Color = color };
    }
    
    public class ChangeRegionNameCommand : ChangeSelectedRegionModelCommand<string>
    {
        protected override Region UpdateRegion(Region region, string newName) => region with { Name = newName };
    }
    
    public class ChangeRegionDescriptionCommand : ChangeSelectedRegionModelCommand<string>
    {
        protected override Region UpdateRegion(Region region, string newDescription) => region with { Description = newDescription };
    }
    
    public class ChangeRegionTypeCommand : ChangeSelectedRegionModelCommand<RegionType>
    {
        protected override Region UpdateRegion(Region region, RegionType newType) => region with { Type = newType };
    }
    
    public class ChangeRegionInvertCommand : ChangeSelectedRegionModelCommand<bool>
    {
        protected override Region UpdateRegion(Region region, bool invert) => region with { Inverted = invert };
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
        [Inject] private ModelCollection<Region> _regions;
        protected override Unit Apply(RegionInCreationState state, SingleActionParam<Region> param)
        {
            var region = param.Value;
            _regions.Add(region);
            state.Builder.Build(region.Id.ToRef(_regions));
            state.PlanetMono.UpdatePlanetMeshes();
            state.PlanetMono.ToEditPlanetState();
            Destroy(state.Builder.gameObject);
            return default;
        }
    }
    
}