#nullable enable
using Game.Planet_.Parts.State;

namespace GameController.Queries
{
    public class PlanetNameQuery : StateDependantQuery<string, IContinentState>
    { 
        public override string Apply(IContinentState state) => PlanetMono.Planet.Name;
    }
}