#nullable enable
using Game.Planet_.Parts.State;
using Game.Region_;

namespace GameController.Queries
{
    public class RegionBuilderQuery : StateDependantQuery<RegionMonoBuilder,RegionInCreationState>
    {
        public override RegionMonoBuilder Apply(RegionInCreationState state) => state.Builder;
    }
}