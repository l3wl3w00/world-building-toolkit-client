#nullable enable
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
using Common.Triggers.GameController;
using Game;
using GameController.Queries;
using InGameUi.Util;
using Zenject;

namespace InGameUi.Button.Event_
{
    public class CreateHistoricalEventOnServerButton : ButtonActionTrigger<CreateHistoricalEventOnServerCommand>
    {
    }

    public class CreateHistoricalEventOnServerCommand : 
        PostApiCallingCommand<NoActionParam,CreateHistoricalEventDto, HistoricalEventDto>,
        IResponseProcessStrategy<HistoricalEventDto>
    {
        [Inject] private HistoricalEventBuilderHolder _builderHolder;
        [Inject] private CreateHistoricalEventCommand _createHistoricalEventCommand;
        [Inject] private SignalBus _signalBus;
        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams)
        {
            var builder = _builderHolder.Builder;
            return endpointFactory.AddHistoricalEvent(builder.Region);
        }

        protected override CreateHistoricalEventDto GetRequestDto(NoActionParam buttonParams)
        {
            var builder = _builderHolder.Builder;
            return new CreateHistoricalEventDto
            (
                Name: builder.Name,
                Description: builder.Description,
                RelativeStart: builder.Beginning,
                RelativeEnd: builder.End,
                DefaultCalendarId: builder.DefaultCalendar.Value
            );
        }

        protected override IResponseProcessStrategy<HistoricalEventDto> GetResponseProcessStrategy(
            NoActionParam buttonParams) => this;

        public void OnSuccess(HistoricalEventDto responseDto)
        {
            _builderHolder.Builder.WithId(responseDto.Id.ToTypesafe<HistoricalEvent>());
            _createHistoricalEventCommand.OnTriggered(new());
            _signalBus.StateChanged();
        }

        public void OnFail(ErrorResponse error) => error.DisplayToUi();
    }
}