#nullable enable
using Common;
using Common.Model.Builder;
using UnityEngine;
using Zenject;

namespace InGameUi.InputFiller.Calendar_
{
    public class CreateCalendarFirstYearInputFiller : TextOptInputFiller<GetCalendarBuilderFirstYear>
    {
        
    }
    public class GetCalendarBuilderFirstYear : MonoBehaviour, IQuery<Option<string>>
    {
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder;
        public Option<string> Get() => _calendarBuilderHolder.BuilderOpt.NullOr(b => b.FirstYear.ToString());
    }
}