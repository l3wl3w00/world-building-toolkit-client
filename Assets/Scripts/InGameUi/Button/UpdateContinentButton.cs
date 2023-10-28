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
using GameController.Commands;
using GameController.Queries;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace InGameUi.Button
{
    public class UpdateContinentCommand : ApiCallingCommand<NoActionParam, PatchContinentDto, ContinentDto>, IResponseProcessStrategy<ContinentDto>
        // HudButtonControl<NoActionParam>, IContinentStateOperation<SelectedContinentState, Unit>
    {
        [Inject] private SelectedContinentQuery _selectedContinentQuery;
        [Inject] private UpdateSelectedContinentModelCommand _continentModelCommand;
        // private Option<WorldBuildingApiClient> _client = Option<WorldBuildingApiClient>.None;
        //
        // protected override void OnStart()
        // {
        //     base.OnStart();
        //     _client = new WorldBuildingApiClient(PlayerPrefs.GetString(AuthConstants.GoogleTokenKey));
        // }
        //
        // protected override void OnClickedTypesafe(NoActionParam param)
        // {
        //     PlanetMono.ApplyStateOperation(this);
        // }
        //
        // public Unit Apply(SelectedContinentState state)
        // {
        //     var texts = FindObjectsOfType<TextMeshProUGUI>().ToOption().Value;
        //     var nameInput = texts.Single(t => t.name == "NameInput");
        //     var descriptionInput = texts.Single(t => t.name == "DescriptionInput");
        //     
        //     var invertToggle = FindObjectsOfType<Toggle>().Single(t => t.name == "Inverted");
        //     var continentName = nameInput.text.Replace("\u200b", "");
        //     var continentDescription = descriptionInput.text.Replace("\u200b", "");
        //     
        //     var inverted = invertToggle.isOn;
        //
        //     var dto = new PatchContinentDto(continentName, continentDescription, inverted);
        //     
        //     _client
        //         .ExpectNotNull(nameof(_client), (Func<SelectedContinentState,Unit>)Apply)
        //         .PatchContinent(
        //             state.SelectedContinent.Continent.Id,
        //             dto,
        //             new ContinentUpdateResponseProcessor(state.SelectedContinent))
        //         .StartCoroutine(this);
        //     return new Unit();
        // }
        //
        // public Unit OnStateNotT(IContinentState state)
        // {
        //     Debug.LogError("No continent was selected when trying to update continent");
        //     return new Unit();
        // }
        //
        // private void OnContinentPatchSuccessful(ContinentDto newContinentDto,
        //     ContinentMonoBehaviour selectedContinent)
        // {
        //
        // }
        //
        // private record ContinentUpdateResponseProcessor(ContinentMonoBehaviour selectedContinent) 
        //     : IResponseProcessStrategy<ContinentDto>
        // {
        //     public void OnSuccess(ContinentDto responseDto)
        //     {
        //         selectedContinent.Continent = responseDto.ToModel();
        //         Debug.Log("Successful update!");
        //     }
        //
        //     public void OnFail(ErrorResponse error)
        //     {
        //         
        //     }
        // }
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

        public void OnSuccess(ContinentDto responseDto) => 
            _continentModelCommand.OnTriggered(new(responseDto.ToModel()));

        public void OnFail(ErrorResponse error)
        {
            
        }
    }

    public class IsExternalInit { }
}