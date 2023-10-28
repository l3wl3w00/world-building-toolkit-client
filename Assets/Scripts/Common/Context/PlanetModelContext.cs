#nullable enable
using Common.Model;
using UnityEngine;

namespace Common.Context
{
    public class PlanetModelContext : MonoBehaviour
    {
        public Planet Planet { get; set; }
        public ModelCollection<Continent> Continents { get; set; }
        public ModelCollection<Region> Regions { get; set; }
    }
}