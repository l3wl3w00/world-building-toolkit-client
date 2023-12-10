#nullable enable
using Game.Planet_.Parts.State;

namespace GameController.Queries
{
    public class SelectedRegionNameQuery : StateDependantQuery<string,SelectedRegionState>
    {
        public override string Apply(SelectedRegionState state) => state.RegionMono.Region.Name;
    }
    
    public class SelectedRegionInvertedQuery : StateDependantQuery<bool,SelectedRegionState>
    {
        public override bool Apply(SelectedRegionState state) => state.RegionMono.Region.Inverted;
    }
}