#nullable enable
using Common;
using Common.Model.Builder;
using UnityEngine;
using Zenject;

namespace InGameUi.InputFiller.Calendar_
{
    public class CreateCalendarNameInputFiller : TextOptInputFiller<GetCalendarBuilderName>
    {
        
    }
    public class GetCalendarBuilderName : MonoBehaviour, IQuery<Option<string>>
    {
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder;
        public Option<string> Get() => _calendarBuilderHolder.BuilderOpt.NullOr(b => b.Name);
    }


}