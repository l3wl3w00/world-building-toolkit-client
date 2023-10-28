#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Dto
{
    public record WorldSummaryDtos(List<WorldSummaryDto> value) : JsonSerializable<WorldSummaryDtos>;
    public record WorldSummaryDto(Guid Id, string Name) : JsonSerializable<WorldSummaryDto>;
    public record CreateWorldDto(string Name, string Description, ColorDto LandColor, ColorDto AntiLandColor)
        : JsonSerializable<CreateWorldDto>;

    public record PatchWorldDto : JsonSerializable<PatchWorldDto>
    {
        public string? Name { get; init;}
        public string? Description { get; init;}
        public ColorDto? LandColor { get; init;}
        public ColorDto? AntiLandColor { get; init;}
    }
    public record WorldDetailedDto(
        Guid Id,
        string Name,
        string Description,
        ColorDto LandColor,
        ColorDto AntiLandColor,
        List<ContinentDto> Continents,
        List<CalendarDto> Calendars
        )
        : JsonSerializable<WorldDetailedDto>
    {
    }
}