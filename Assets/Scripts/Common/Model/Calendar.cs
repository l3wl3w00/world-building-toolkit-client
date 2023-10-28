#nullable enable
using System;
using System.Collections.Generic;
using Common.Model.Abstractions;

namespace Common.Model
{
    public partial record Calendar 
    (
        IdOf<Calendar> Id,
        IdOf<Planet> PlanetId ,
        string Name,
        string Description,
        uint FirstYear,
        List<YearPhase> YearPhases
    ) : IModel<Calendar>;
    
    public record YearPhase(string Name, uint NumberOfDays);
}