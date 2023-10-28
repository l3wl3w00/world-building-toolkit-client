#nullable enable
using System.Linq;
using Client;
using Client.Dto;
using Client.EndpointUtil;
using Client.Request;
using Client.Response;
using Common;
using Common.ButtonBase;
using Common.Constants;
using GameController.Commands;
using GameController.Queries;
using TMPro;
using UnityEngine;
using Zenject;

namespace InGameUi.Button
{
    public class UpdateWorldButton : ApiCallingCommand<NoActionParam, PatchWorldDto, WorldDetailedDto>, IResponseProcessStrategy<WorldDetailedDto>
        // HudButtonControl<NoActionParam>, IResponseProcessStrategy<WorldDetailedDto>
    {
        [Inject] private PlanetQuery _planetQuery;
        [Inject] private UpdateMeshesCommand _updateMeshesCommand;
        // private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;
        //
        // protected override void OnStart()
        // {
        //     _client = new WorldBuildingApiClient(
        //             PlayerPrefs.GetString(AuthConstants.GoogleTokenKey))
        //         .ToOption();
        // }
        //
        // protected override void OnClickedTypesafe(NoActionParam actionParam)
        // {
        //     var texts = FindObjectsOfType<TextMeshProUGUI>();
        //     
        //     //TODO query from model object, and set that value immediately when changing the name
        //     var worldName = texts.Single(t => t.name == "NameInput").text.Replace("\u200b", "");
        //     var description = texts.Single(t => t.name == "DescriptionInput").text.Replace("\u200b", "");
        //     
        //     var dto = new PatchWorldDto
        //     {
        //         Name = worldName,
        //         Description = description,
        //     };
        //     _client
        //         .ExpectNotNull($"client was uninitialized in {nameof(UpdateWorldButton)}")
        //         .UpdateWorld(PlanetMono.Planet.Id, dto, this)
        //         .StartCoroutine(this);
        // }
        //
        // public void OnSuccess(WorldDetailedDto responseDto)
        // {
        //     PlanetMono.UpdatePlanetMeshes();
        // }
        //
        // public void OnFail(ErrorResponse error)
        // {
        //     
        // }
        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams)
        {
            return endpointFactory.UpdateWorld(_planetQuery.Get().Id);
        }

        protected override HttpMethod Method => HttpMethod.Patch;
        protected override PatchWorldDto GetRequestDto(NoActionParam buttonParams)
        {
            var texts = FindObjectsOfType<TextMeshProUGUI>();
            
            //TODO query from model object, and set that value immediately when changing the name
            var worldName = texts.Single(t => t.name == "NameInput").text.Replace("\u200b", "");
            var description = texts.Single(t => t.name == "DescriptionInput").text.Replace("\u200b", "");
            
            return new PatchWorldDto
            {
                Name = worldName,
                Description = description,
            };
        }

        protected override IResponseProcessStrategy<WorldDetailedDto> GetResponseProcessStrategy(NoActionParam buttonParams) 
            => this;

        public void OnSuccess(WorldDetailedDto responseDto)
        {
            _updateMeshesCommand.OnTriggered(new());
        }

        public void OnFail(ErrorResponse error)
        {
            
        }
    }
}