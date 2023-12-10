using System.Linq;
using Client.Dto;
using Client.EndpointUtil;
using Client.Request;
using Client.Response;
using Common;
using Common.ButtonBase;
using Common.Constants;
using Common.Generated;
using Common.Triggers;
using Common.Triggers.GameController;
using Common.Utils;
using InGameUi;
using InGameUi.Util;
using TMPro;
using UI.IntroScreen.Command;
using UnityEngine;

namespace UI.Register.Button
{
    public class RegisterButton : ButtonActionTrigger<RegisterCommand>
    {
        
    }

    public class RegisterCommand : PostApiCallingCommand<NoActionParam, RegisterDto, UserIdentityDto>, IResponseProcessStrategy<UserIdentityDto>
    {
        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, NoActionParam buttonParams) =>
            endpointFactory.Register();

        protected override RegisterDto GetRequestDto(NoActionParam buttonParams)
        {
            var texts = FindObjectsOfType<TMP_InputField>().ToOption().Value;
            var email = texts.Single(t => t.name == "EmailInput").text;
            var username = texts.Single(t => t.name == "UsernameInput").text;
            var password = texts.Single(t => t.name == "PasswordInput").text;
            
            return new RegisterDto(email, username, password);
        }

        protected override IResponseProcessStrategy<UserIdentityDto> GetResponseProcessStrategy(NoActionParam buttonParams)
        {
            return this;
        }

        public void OnSuccess(UserIdentityDto responseDto)
        {
            Prefab.DisplayShortMessage
                .InstantiateAndExpectComponent<DisplayShortMessageController>()
                .Construct("Register Successful!", MessageType.Info, 5);
        }

        public void OnFail(ErrorResponse error)
        {
            error.LogError();
            error.DisplayToUi();
        }
    }
}