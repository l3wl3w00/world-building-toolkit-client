#nullable enable
using Game.Planet_.Parts.State;
using UnityEngine;

namespace GameController.Queries
{
    public class SelectedRegionColorQuery : StateDependantQuery<Color,SelectedRegionState>
    {
        public override Color Apply(SelectedRegionState state) => state.RegionMono.Region.Color;
    }
}