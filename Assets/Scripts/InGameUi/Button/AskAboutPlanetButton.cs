#nullable enable
using Client.Dto;
using Client.EndpointUtil;
using Client.Request;
using Client.Response;
using Common.ButtonBase;
using Common.Model;
using Common.Triggers;
using Game.Planet_.Parts;
using InGameUi.Util;
using UnityEngine;
using Zenject;

namespace InGameUi.Button
{
    public class AskAboutPlanetButton : UserControlledActionTrigger<UnityEngine.UI.Button>
    {
        [SerializeField] private AiAssistantUiManager _aiAssistantUiManager;
        [Inject] private AskAboutPlanetCommand _command;
        protected override void RegisterListener(UnityEngine.UI.Button component)
        {
            component.onClick.AddListener(() => _command.OnTriggered(_aiAssistantUiManager.ToActionParam()));
        }
    }

    public class AskAboutPlanetCommand : PostApiCallingCommand<SingleActionParam<AiAssistantUiManager>, AiPromptDto, AiAnswerDto>, IResponseProcessStrategy<AiAnswerDto>
    {
        [Inject] private PlanetMonoBehaviour _planetMonoBehaviour;
        private AiAssistantUiManager _aiAssistantUiManager;

        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, SingleActionParam<AiAssistantUiManager> buttonParams)
        {
            _aiAssistantUiManager = buttonParams.Value;
            return endpointFactory.AskAboutPlanet(_planetMonoBehaviour.Planet.Id);
        }

        protected override AiPromptDto GetRequestDto(SingleActionParam<AiAssistantUiManager> buttonParams)
        {
            return new AiPromptDto(_aiAssistantUiManager.Prompt);
        }

        protected override IResponseProcessStrategy<AiAnswerDto> GetResponseProcessStrategy(SingleActionParam<AiAssistantUiManager> buttonParams)
            => this;

        public void OnSuccess(AiAnswerDto responseDto)
        {
            _aiAssistantUiManager.SetAnswer(responseDto.Answer);
        }

        public void OnFail(ErrorResponse error)
        {
            error.DisplayToUi();
        }
    }
}