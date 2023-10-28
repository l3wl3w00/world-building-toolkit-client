#nullable enable
namespace Client.Dto
{
    public record PlanetCoordinateDto
        (float Radius, float Polar, float Azimuthal) : JsonSerializable<PlanetCoordinateDto>;
}