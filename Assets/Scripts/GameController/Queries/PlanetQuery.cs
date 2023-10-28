#nullable enable
using Common.Model;
using Game.Planet_.Parts.State;

namespace GameController.Queries
{
    public class PlanetQuery : StateDependantQuery<Planet, IContinentState>
    { 
        public override Planet Apply(IContinentState state) => PlanetMono.Planet;
    }
}