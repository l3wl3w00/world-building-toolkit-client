#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Game.Planet_.Parts.State;
using GameController.Commands;
using GameController.Queries;
using InGameUi.Util;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace InGameUi.Button
{
    public class ContinentCreateOkButton : ButtonActionTrigger<ContinentCreateOkCommand> { }

    public class ContinentCreateOkCommand : ApiCallingCommand<
        NoActionParam,
        CreateContinentDto,
        ContinentDto>,
        IResponseProcessStrategy<ContinentDto>
    {
        [Inject] private CreateContinentWithParentCommand _createContinentWithParentCommand = null!; //Asserted in OnStart
        [Inject] private ContinentInCreationBuilderQuery _builderQuery = null!; //Asserted in OnStart
        [Inject] private PlanetQuery _planetQuery = null!; //Asserted in OnStart
        protected override void OnStart()
        {
            base.OnStart();
            NullChecker.AssertNoneIsNullInType(GetType(), _createContinentWithParentCommand, _builderQuery, _planetQuery);
        }

        protected override HttpMethod Method => HttpMethod.Post;

        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams) 
            => endpointFactory.CreateContinent(_planetQuery.Get().Id);

        protected override CreateContinentDto GetRequestDto(NoActionParam buttonParams) => 
            MapperMethods.ToCreateContinentDto(_builderQuery.Get().ControlPoints, _builderQuery.Get().ParentId);
        protected override void BeforeRequestIsSent()
        {
            _builderQuery.Get().OnFinishedBuilding();
        }
        protected override IResponseProcessStrategy<ContinentDto> GetResponseProcessStrategy(NoActionParam buttonParams) => this;


        public void OnSuccess(ContinentDto responseDto)
        {
            var continent = responseDto.ToModel();
            _createContinentWithParentCommand.OnTriggered(new(continent));
        }

        public void OnFail(ErrorResponse error) => error.DisplayToUi();
    }
}