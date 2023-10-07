#nullable enable
using System;

namespace Game.Client.Dto
{
    public record PlanetCoordinateDto : JsonSerializable<PlanetCoordinateDto>
    {
        public float Radius { get; set; }
        public float Polar { get; set; }
        public float Azimuthal { get; set; }
    }
}