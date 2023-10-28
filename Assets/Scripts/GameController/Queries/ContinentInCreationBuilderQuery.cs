#nullable enable
using Common.Model.Abstractions;
using Game.Continent_;
using Game.Planet_.Parts.State;

namespace GameController.Queries
{
    public class ContinentInCreationBuilderQuery : StateDependantQuery<ContinentMonoBuilder,ContinentInCreationState>
    {
        public override ContinentMonoBuilder Apply(ContinentInCreationState state) => state.Builder;
    }
}