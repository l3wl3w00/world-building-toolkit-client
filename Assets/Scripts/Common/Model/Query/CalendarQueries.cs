using System.Linq;
using Common.Model.Abstractions;
using UnityEngine;
using Zenject;

namespace Common.Model.Query
{
    public class GetCalendarById : MonoBehaviour, IQuery<IdOf<Calendar>, Calendar>
    {
        [Inject] private ModelCollection<Calendar> _calendars;
        public Calendar Get(IdOf<Calendar> id) => _calendars
            .SingleOrDefault(c => c.Id == id)
            .ToOption()
            .ExpectNotNull($"No calendar was found with id {id}");
    }
}