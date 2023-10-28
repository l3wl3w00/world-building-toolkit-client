#nullable enable
using Game.Planet_.Parts.State;
using UnityEngine;

namespace GameController.Queries
{
    public class PlanetLandColorQuery : StateDependantQuery<Color, IContinentState>
    { 
        public override Color Apply(IContinentState state) => PlanetMono.Planet.LandColor;
    }
}