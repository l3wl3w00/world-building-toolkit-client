#nullable enable
using Client;
using Client.Dto;
using Client.EndpointUtil;
using Client.Mapper;
using Client.Request;
using Client.Response;
using Common.ButtonBase;
using Common.Model;
using Common.Utils;
using GameController.Queries;
using InGameUi.Util;
using UnityEngine;
using Zenject;

namespace InGameUi.Button
{
    public class GetCalendarsCommand : GetApiCallingCommand<NoActionParam, CalendarDto>, IResponseProcessStrategy<CalendarDto>
    {
        [Inject] private PlanetQuery _planetQuery = null!; // Asserted in OnStart
        [Inject] private ModelCollection<Calendar> _calendars = null!; // Asserted in OnStart
        
        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _planetQuery, _calendars);
        }
        
        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams) =>
            endpointFactory.GetCalendars(_planetQuery.Get().Id);

        protected override IResponseProcessStrategy<CalendarDto> GetResponseProcessStrategy(NoActionParam buttonParams) => 
            this;

        public void OnSuccess(CalendarDto responseDto)
        {
            _calendars.Add(responseDto.ToModel());
        }

        public void OnFail(ErrorResponse error) => error.DisplayToUi();
    }
    
    public class GetCalendarCommand : GetApiCallingCommand<NoActionParam, CalendarDto>, IResponseProcessStrategy<CalendarDto>
    {
        [Inject] private PlanetQuery _planetQuery;
        [Inject] private ModelCollection<Calendar> _calendars;
        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams) =>
            endpointFactory.GetCalendars(_planetQuery.Get().Id);

        protected override IResponseProcessStrategy<CalendarDto> GetResponseProcessStrategy(NoActionParam buttonParams) => 
            this;

        public void OnSuccess(CalendarDto responseDto)
        {
            _calendars.Add(responseDto.ToModel());
        }

        public void OnFail(ErrorResponse error) => error.DisplayToUi();
    }
    
    public class AddCalendarCommand : PostApiCallingCommand<NoActionParam, CreateCalendarDto, CalendarDto>, IResponseProcessStrategy<CalendarDto>
    {
        [Inject] private PlanetQuery _planetQuery;
        [Inject] private ModelCollection<Calendar> _calendars;
        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams) =>
            endpointFactory.AddCalendar(_planetQuery.Get().Id);
        
        protected override CreateCalendarDto GetRequestDto(NoActionParam buttonParams)
        {
            throw new System.NotImplementedException();
        }

        protected override IResponseProcessStrategy<CalendarDto> GetResponseProcessStrategy(NoActionParam buttonParams) => 
            this;

        public void OnSuccess(CalendarDto responseDto)
        {
            _calendars.Add(responseDto.ToModel());
        }

        public void OnFail(ErrorResponse error) => error.DisplayToUi();
    }
}