#nullable enable
using System;
using System.Linq;
using Client;
using Client.Dto;
using Client.EndpointUtil;
using Client.Mapper;
using Client.Request;
using Client.Response;
using Common;
using Common.ButtonBase;
using Common.Constants;
using Common.Model;
using Common.Model.Abstractions;
using Common.Triggers;
using Common.Utils;
using GameController.Commands;
using InGameUi.Util;
using TMPro;
using UnityEngine;
using Zenject;

namespace InGameUi.Button
{
    public class CreateWorldButton : ButtonActionTrigger<CreateWorldCommand>
    {
        
    }
    public class CreateWorldCommand : ApiCallingCommand<NoActionParam, CreateWorldDto, WorldDetailedDto>, IResponseProcessStrategy<WorldDetailedDto>
    {

        [Inject] private CreatePlanetCommand _createPlanetCommand = null!; //Asserted in OnStart
        
        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _createPlanetCommand);
        }
        
        protected override WorldBuildingApiEndpoint
            GetEndpoint(EndpointFactory endpointFactory, NoActionParam actionParam) =>
                endpointFactory.CreateWorld();

        protected override HttpMethod Method => HttpMethod.Post;
        protected override CreateWorldDto GetRequestDto(NoActionParam actionParam)
        {
            var texts = FindObjectsOfType<TMP_InputField>().ToOption().Value;
            var worldName = texts.Single(t => t.name == "NameInput").text.Replace("\u200b", "");
            var description = texts.Single(t => t.name == "DescriptionInput").text.Replace("\u200b", "");
            var daysInAYearText = texts.Single(t => t.name == "DaysInAYearInput").text.Replace("\u200b", "");
            var dayLengthText = texts.Single(t => t.name == "DayLengthInput").text.Replace("\u200b", "");
            var daysInAYear = daysInAYearText.ToUInt();
            var dayLength = TimeSpan.FromHours(dayLengthText.ToDouble());
            
            return new CreateWorldDto(
                worldName,
                description,
                new ColorDto(70, 150, 50),
                new ColorDto(50, 126, 240), 
                dayLength, 
                daysInAYear);
        }

        protected override IResponseProcessStrategy<WorldDetailedDto> GetResponseProcessStrategy(NoActionParam actionParam) => this;

        public void OnSuccess(WorldDetailedDto responseDto)
        {
            var newPlanet = responseDto.ToModel();
            var events = responseDto.Continents.SelectMany(c => c.Regions).SelectMany(r => r.Events).ToModels().ToList();
            _createPlanetCommand.OnTriggered(
                new(newPlanet,responseDto.Continents.ToModels(), responseDto.Calendars.ToModels(), events));
        }

        public void OnFail(ErrorResponse error) => error.DisplayToUi();
    }
}