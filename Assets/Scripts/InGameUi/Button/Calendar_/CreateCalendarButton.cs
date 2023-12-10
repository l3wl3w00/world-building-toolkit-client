#nullable enable
using Client;
using Client.Dto;
using Client.EndpointUtil;
using Client.Mapper;
using Client.Request;
using Client.Response;
using Common.ButtonBase;
using Common.Model;
using Common.Model.Abstractions;
using Common.Model.Builder;
using Common.Model.Command;
using Common.Triggers;
using Common.Utils;
using Game;
using Game.Common.Holder;
using Game.Planet_.Parts;
using GameController;
using InGameUi.Util;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using Zenject;

namespace InGameUi.Button.Calendar_
{
    public class CreateCalendarButton : ButtonActionTrigger<CreateAndPersistCalendarInCreationCommand>
    {
        
    }

    public class CreateAndPersistCalendarInCreationCommand 
        : PostApiCallingCommand<NoActionParam, CreateCalendarDto, CalendarDto>, IResponseProcessStrategy<CalendarDto>
    {
        [Inject] private PlanetMonoBehaviour _planetMono = null!; // Asserted in OnStart
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder = null!; // Asserted in OnStart
        [Inject] private CreateCalendarCommand _createCalendarCommand = null!; // Asserted in OnStart
        [Inject] private SignalBus _signalBus = null!; // Asserted in OnStart
        [Inject] private UiController _uiController = null!; // Asserted in OnStart

        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType( GetType(),
                _planetMono, 
                _calendarBuilderHolder, 
                _createCalendarCommand, 
                _signalBus,
                _uiController);
        }
        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams) =>
            endpointFactory.CreateCalendar(_planetMono.Planet.Id);
        protected override CreateCalendarDto GetRequestDto(NoActionParam buttonParams)
        {
            var builder = _calendarBuilderHolder.Builder;
            return new CreateCalendarDto(builder.Name, builder.Description, builder.FirstYear, builder.YearPhases);
        }

        protected override IResponseProcessStrategy<CalendarDto> GetResponseProcessStrategy(NoActionParam buttonParams)
            => this;

        public void OnSuccess(CalendarDto responseDto)
        {
            _calendarBuilderHolder.Builder
                .WithId(responseDto.Id.ToTypesafe<Calendar>())
                .WithPlanetId(_planetMono.Planet.Id);
            _createCalendarCommand.OnTriggered(new());
            _uiController.CloseUi(UiType.CreateCalendar);
            _signalBus.Fire<StateChangedSignal>();
        }

        public void OnFail(ErrorResponse error) => error.DisplayToUi();
    }
}