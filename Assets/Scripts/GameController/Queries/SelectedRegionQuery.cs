#nullable enable
using Common.Model;
using Game.Planet_.Parts.State;

namespace GameController.Queries
{
    public class SelectedRegionQuery : StateDependantQuery<Region,SelectedRegionState>
    {
        public override Region Apply(SelectedRegionState state) => state.RegionMono.Region;
    }
}