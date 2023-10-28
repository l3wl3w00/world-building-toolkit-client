#nullable enable
using Common.Model.Abstractions;

namespace Common.Model
{
    public partial record HistoricalEvent(IdOf<HistoricalEvent> Id) : IModel<HistoricalEvent>
    {
        
    }
}