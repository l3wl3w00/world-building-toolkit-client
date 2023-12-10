#nullable enable
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Model;
using UnityEngine;
using Zenject;

namespace GameController.Queries
{
    public class GetEventsForSelectedRegion : MonoBehaviour, IQuery<List<HistoricalEvent>>
    {
        [Inject] private SelectedRegionQuery _selectedRegionQuery;
        [Inject] private ModelCollection<HistoricalEvent> _events;
        public List<HistoricalEvent> Get()
        {
            var selectedRegionId = _selectedRegionQuery.Get().Id;
            return _events.Where(e => e.Region == selectedRegionId).ToList();
        }
    }
}