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
using Common.Triggers;
using Common.Utils;
using Game.Planet_.Parts.State;
using GameController.Commands;
using GameController.Queries;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InGameUi.Button
{
    public class RegionCreateOkButton : ButtonActionTrigger<RegionCreateOkCommand> { };

    public class RegionCreateOkCommand : ApiCallingCommand<
        NoActionParam,
        CreateRegionDto,
        RegionDto>, 
        IResponseProcessStrategy<RegionDto>
    {
        [Inject] private RegionBuilderQuery _regionBuilderQuery = null!; //Asserted in OnStart
        [Inject] private CreateRegionCommand _createRegionCommand = null!; //Asserted in OnStart

        protected override void OnStart()
        {
            base.OnStart();
            NullChecker.AssertNoneIsNullInType(GetType(), _regionBuilderQuery, _createRegionCommand);
        }
        
        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams) 
            => endpointFactory.AddRegion(_regionBuilderQuery.Get().ContinentId);

        protected override HttpMethod Method => HttpMethod.Post;
        protected override CreateRegionDto GetRequestDto(NoActionParam buttonParams)
        {
            var dropdowns = FindObjectsOfType<TMP_Dropdown>().ToOption().Value;
            var dropdown = dropdowns.Single(t => t.name == "TypeDropdown");
            var builder = _regionBuilderQuery.Get(); 
            builder.OnFinishedBuilding();
            return MapperMethods.ToCreateRegionDto(
                builder.ControlPoints, 
                (RegionType) dropdown.value, 
                new Color(1f,1f,1f,2f));
        }

        protected override IResponseProcessStrategy<RegionDto> GetResponseProcessStrategy(NoActionParam buttonParams)
            => this;

        public void OnSuccess(RegionDto responseDto)
        {
            _createRegionCommand.OnTriggered(new(responseDto.ToModel()));
        }

        public void OnFail(ErrorResponse error)
        {
            
        }
    }
}