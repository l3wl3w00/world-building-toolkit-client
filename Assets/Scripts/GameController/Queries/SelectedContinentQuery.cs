#nullable enable
using Common.Model;
using Game.Planet_.Parts.State;

namespace GameController.Queries
{
    public class SelectedContinentQuery : StateDependantQuery<Continent,SelectedContinentState>
    {
        public override Continent Apply(SelectedContinentState state) => state.SelectedContinent.Continent;
    }
}