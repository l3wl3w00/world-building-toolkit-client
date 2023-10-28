#nullable enable
using Game.Planet_.Parts.State;

namespace GameController.Queries
{
    public class SelectedContinentDescriptionQuery : StateDependantQuery<string,SelectedContinentState>
    {
        public override string Apply(SelectedContinentState state) => state.SelectedContinent.Continent.Description;
    }
}