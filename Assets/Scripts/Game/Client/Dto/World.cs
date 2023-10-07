#nullable enable
using System;
using System.Collections.Generic;

namespace Game.Client.Dto
{
    public record WorldSummaryDto : JsonSerializable<WorldSummaryDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
    }

    public record CreateWorldDto : JsonSerializable<CreateWorldDto>
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public record WorldDetailedDto : JsonSerializable<WorldDetailedDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public List<ContinentDto> Continents { get; set; } = new();
    }
}