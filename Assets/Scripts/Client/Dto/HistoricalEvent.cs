#nullable enable
using System;
using Common.Model;
using Common.Model.Abstractions;

namespace Client.Dto
{
    public record CreateHistoricalEventDto(
        string Name, 
        string Description, 
        Date RelativeStart, 
        Date RelativeEnd,
        Guid DefaultCalendarId) : JsonSerializable<CreateHistoricalEventDto>;
    
    public record HistoricalEventDto(
        Guid Id,
        string Name, 
        string Description, 
        Date RelativeStart, 
        Date RelativeEnd,
        Guid RegionId,
        Guid DefaultCalendarId
        ) : JsonSerializable<HistoricalEventDto>;
}