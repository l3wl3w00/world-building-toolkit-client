#nullable enable
using System.Linq;
using Client;
using Client.Dto;
using Client.EndpointUtil;
using Client.Mapper;
using Client.Request;
using Client.Response;
using Common;
using Common.ButtonBase;
using Common.Triggers;
using Common.Utils;
using Game;
using GameController.Commands;
using GameController.Queries;
using InGameUi.Util;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace InGameUi.Button
{
    public class UpdateContinentButton : ButtonActionTrigger<UpdateContinentCommand> { }

    public class UpdateContinentCommand : ApiCallingCommand<NoActionParam, PatchContinentDto, ContinentDto>, IResponseProcessStrategy<ContinentDto>
    {
        [Inject] private SelectedContinentQuery _selectedContinentQuery = null!; // Asserted in OnStart
        [Inject] private UpdateSelectedContinentModelCommand _continentModelCommand = null!; // Asserted in OnStart
        [Inject] private SignalBus _signalBus = null!; // Asserted in OnStart

        protected override void OnStart()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), _selectedContinentQuery, _continentModelCommand, _signalBus);
        }

        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams) =>
            endpointFactory.PatchContinent(_selectedContinentQuery.Get().Id);

        protected override HttpMethod Method => HttpMethod.Patch;
        protected override PatchContinentDto GetRequestDto(NoActionParam buttonParams)
        {
            var texts = FindObjectsOfType<TextMeshProUGUI>().ToOption().Value;
            var nameInput = texts.Single(t => t.name == "NameInput");
            var descriptionInput = texts.Single(t => t.name == "DescriptionInput");
            
            var invertToggle = FindObjectsOfType<Toggle>().Single(t => t.name == "Inverted");
            var continentName = nameInput.text.Replace("\u200b", "");
            var continentDescription = descriptionInput.text.Replace("\u200b", "");
            
            var inverted = invertToggle.isOn;
            
            return new PatchContinentDto(continentName, continentDescription, inverted);
        }

        protected override IResponseProcessStrategy<ContinentDto> GetResponseProcessStrategy(NoActionParam buttonParams) 
            => this;

        public void OnSuccess(ContinentDto responseDto)
        {
            _continentModelCommand.OnTriggered(new(responseDto.ToModel()));
            _signalBus.StateChanged();
        }

        public void OnFail(ErrorResponse error) => error.DisplayToUi();
    }

    public class IsExternalInit { }
}