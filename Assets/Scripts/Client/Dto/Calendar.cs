#nullable enable
using System;
using System.Collections.Generic;
using Common.Model;

namespace Client.Dto
{
    public record CalendarDtos(List<CalendarDto> Value) : JsonSerializable<CalendarDto>;
    public record CalendarDto(Guid Id, string Name, string Description, ulong FirstYear, Guid PlanetId, List<YearPhase> YearPhases)
        : JsonSerializable<CalendarDto>;
    
    public record CreateCalendarDto(string Name, string Description, ulong FirstYear, List<YearPhase> YearPhases) 
        : JsonSerializable<CreateCalendarDto>;
}