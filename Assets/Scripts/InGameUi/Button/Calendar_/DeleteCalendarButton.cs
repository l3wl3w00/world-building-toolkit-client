#nullable enable
using Client.EndpointUtil;
using Client.Request;
using Client.Response;
using Common.ButtonBase;
using Common.Model;
using Common.Model.Abstractions;
using Common.Triggers;
using Common.Triggers.GameController;
using Game;
using Game.Common;
using Game.Planet_.Parts;
using GameController;
using InGameUi.Util;
using UnityEngine.Events;
using Zenject;

namespace InGameUi.Button.Calendar_
{
    public class DeleteCalendarButton : ButtonActionTrigger
    {
        [Inject] private DeleteCalendarOnServerCommand _command;
        protected override UnityAction GetClickListener()
        {
            return () => _command.OnTriggered(gameObject.GetIdInParent<Calendar>().ToActionParam());
        }
    }

    public class DeleteCalendarOnServerCommand : DelApiCallingCommand<SingleActionParam<IdOf<Calendar>>>
    {
        [Inject] private PlanetMonoBehaviour _planetMonoBehaviour;
        [Inject] private ModelCollection<Calendar> _calendars;
        [Inject] private SignalBus _signalBus;
        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, SingleActionParam<IdOf<Calendar>> param)
        {
            var planetId = _planetMonoBehaviour.Planet.Id;
            var calendar = param.Value.GetModel(_calendars);

            return endpointFactory.DeleteCalendar(planetId, calendar.Name);
        }

        protected override IResponseProcessStrategy<NoResponseBody> 
            GetResponseProcessStrategy(SingleActionParam<IdOf<Calendar>> buttonParams) =>
            new ResponseProcess(buttonParams.Value, _calendars, _signalBus);

        private class ResponseProcess : IResponseProcessStrategy<NoResponseBody>
        {
            private readonly IdOf<Calendar> _id;
            private readonly ModelCollection<Calendar> _calendars;
            private readonly SignalBus _signalBus;

            public ResponseProcess(IdOf<Calendar> id, ModelCollection<Calendar> calendars, SignalBus signalBus)
            {
                _id = id;
                _calendars = calendars;
                _signalBus = signalBus;
            }

            public void OnSuccess(NoResponseBody responseDto)
            {
                _calendars
                    .RemoveById(_id)
                    .ExpectNotNull($"Calendar with id '{_id}' was not present in the collection when it was attempted to be removed");
                _signalBus.Fire<StateChangedSignal>();
            }
    
            public void OnFail(ErrorResponse error)
            {
                error.DisplayToUi();
            }
        }

    }
}