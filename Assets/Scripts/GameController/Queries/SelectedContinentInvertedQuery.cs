#nullable enable
using Game.Planet_.Parts.State;

namespace GameController.Queries
{
    public class SelectedContinentInvertedQuery : StateDependantQuery<bool,SelectedContinentState>
    {
        public override bool Apply(SelectedContinentState state) => state.SelectedContinent.Inverted;
    }
}