#nullable enable
using System;
using System.Collections.Generic;
using Common.Model;
using UnityEngine;

namespace Client.Dto
{
    public record RegionDto(
        Guid Id,
        Guid ContinentId,
        string Name,
        string Description,
        bool Inverted,
        RegionType RegionType,
        ColorDto Color,
        List<HistoricalEventDto> Events,
        List<PlanetCoordinateDto> Bounds) : JsonSerializable<RegionDto>;


    public record CreateRegionDto(
        string Name,
        string Description,
        RegionType RegionType,
        ColorDto Color,
        List<PlanetCoordinateDto> Bounds) 
        : JsonSerializable<CreateRegionDto>;

    public record PatchRegionDto(
            string? Name,
            string? Description,
            bool? Inverted,
            RegionType? RegionType,
            ColorDto? Color) 
        : JsonSerializable<PatchRegionDto>;
}