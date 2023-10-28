#nullable enable
using Game.Planet_.Parts.State;

namespace GameController.Queries
{
    public class PlanetDescriptionQuery : StateDependantQuery<string, IContinentState>
    { 
        public override string Apply(IContinentState state) => PlanetMono.Planet.Description;
    }
}