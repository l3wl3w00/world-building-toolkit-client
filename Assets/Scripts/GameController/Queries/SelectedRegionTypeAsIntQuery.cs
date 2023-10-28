#nullable enable
using Game.Continent_;
using Game.Planet_.Parts.State;

namespace GameController.Queries
{
    public class SelectedRegionTypeAsIntQuery : StateDependantQuery<int,SelectedRegionState>
    {
        public override int Apply(SelectedRegionState state) => (int) state.RegionMono.Region.Type;
    }
}