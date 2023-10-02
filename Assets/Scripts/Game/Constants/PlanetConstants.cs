#nullable enable
using UnityEngine;

namespace Game.Constants
{
    public class PlanetConstants : IPlanetConstants
    {
        public Vector3 PlanetUp => new(0, 1, 0);
    }
}