#nullable enable
using Game.Planet_.Parts.State;
using UnityEngine;

namespace GameController.Queries
{
    public class PlanetAntiLandColorQuery : StateDependantQuery<Color, IContinentState>
    { 
        public override Color Apply(IContinentState state) => PlanetMono.Planet.AntiLandColor;
    }
}