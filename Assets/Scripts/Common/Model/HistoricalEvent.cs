#nullable enable
using System.Linq;
using Codice.Client.Common.Connection;
using Common.Model.Abstractions;

namespace Common.Model
{
    public partial record HistoricalEvent(
        IdOf<HistoricalEvent> Id,
        IdOf<Region> Region,
        IdOf<Calendar> DefaultCalendar,
        string Name,
        string Description,
        Date Beginning,
        Date End
        ) : IModel<HistoricalEvent>
    {
        public string ToDisplayText(ModelCollection<Calendar> calendars)
        {
            return $"{Name}\n" +
                   $"Calendar: {DefaultCalendar.GetModel(calendars).Name}\n" +
                   $"{Beginning.ToDisplayText()} - {End.ToDisplayText()}";
        }
    }

    public partial record Date(
        int Year,
        string YearPhase,
        uint Day,
        uint Hour,
        uint Minute)
    {
        public static Date Default()
        {
            return new Date(0, "No year phase chosen", 0u, 0u, 0u);
        }

        public string ToDisplayText()
        {
            return $"{Year}, {YearPhase} {Day}. {Hour}:{Minute}";
        }
    }
}