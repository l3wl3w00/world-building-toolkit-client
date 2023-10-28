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
using GameController.Commands;
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

        [Inject] private CreatePlanetCommand _createPlanetCommand;
        protected override WorldBuildingApiEndpoint
            GetEndpoint(EndpointFactory endpointFactory, NoActionParam actionParam) =>
                endpointFactory.CreateWorld();

        protected override HttpMethod Method => HttpMethod.Post;
        protected override CreateWorldDto GetRequestDto(NoActionParam actionParam)
        {
            var texts = FindObjectsOfType<TextMeshProUGUI>().ToOption().Value;
            var worldName = texts.Single(t => t.name == "NameInput").text.Replace("\u200b", "");
            var description = texts.Single(t => t.name == "DescriptionInput").text.Replace("\u200b", "");
            
            return new CreateWorldDto(
                worldName,
                description,
                new ColorDto(70, 150, 50),
                new ColorDto(50, 126, 240));
        }

        protected override IResponseProcessStrategy<WorldDetailedDto> GetResponseProcessStrategy(NoActionParam actionParam) => this;

        public void OnSuccess(WorldDetailedDto responseDto)
        {
            var newPlanet = responseDto.ToModel();
            _createPlanetCommand.OnTriggered(new(newPlanet,responseDto.Continents.ToModels(), responseDto.Calendars.ToModels()));
        }

        public void OnFail(ErrorResponse error)
        {
            
        }
    }


}