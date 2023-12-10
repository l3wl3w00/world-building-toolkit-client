#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Model.Abstractions;

namespace Common.Model
{
    public partial record Calendar 
    (
        IdOf<Calendar> Id,
        IdOf<Planet> PlanetId ,
        string Name,
        string Description,
        ulong FirstYear,
        List<YearPhase> YearPhases
    ) : IModel<Calendar>;

    public record YearPhase(string Name, uint NumberOfDays)
    {
        public static YearPhase Default() => new(string.Empty, 1);
    }
    
    public static class CalendarCollectionUtils
    {
        public static Option<Calendar> GetByNameMaybe(this ModelCollection<Calendar> calendars, string name)
        {
            return calendars.SingleOrDefault(c => c.Name == name).ToOption();
        }
        
        public static Calendar GetByName(this ModelCollection<Calendar> calendars, string name)
        {
            return calendars.SingleOrDefault(c => c.Name == name).ToOption().ExpectNotNull($"No calendar was found with name {name}");
        }
    }
}