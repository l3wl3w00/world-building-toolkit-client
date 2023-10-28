#nullable enable
using System.Collections.Generic;

namespace Common.Model
{
    public record PlanetWithRelatedEntities(Planet Planet, ICollection<Continent> Continents, ICollection<Calendar> Calendars);
}