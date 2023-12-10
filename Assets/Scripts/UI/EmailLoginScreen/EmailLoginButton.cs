using System;
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
using Common.Utils;
using InGameUi.Util;
using TMPro;
using UI.IntroScreen.Button;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.EmailLoginScreen
{
    public class EmailLoginButton : UserControlledActionTrigger<Button>
    {
        [Inject] private LoginCommand _loginCommand;
        protected override void RegisterListener(Button component)
        {
            component.onClick.AddListener(() => _loginCommand.OnTriggered(LoginType.ByEmail.ToActionParam()));
        }
    }

    public class LoginCommand: PostApiCallingCommand<SingleActionParam<LoginType>, LoginDto, UserIdentityDtoWithToken>, IResponseProcessStrategy<UserIdentityDtoWithToken>
    {
        private readonly ITimeProvider _timeProvider = new SystemTimeProvider();

        protected override WorldBuildingApiEndpoint GetEndpoint(EndpointFactory endpointFactory, SingleActionParam<LoginType> buttonParams)
        {
            return endpointFactory.Login();
        }

        protected override LoginDto GetRequestDto(SingleActionParam<LoginType> buttonParams)
        {
            var texts = FindObjectsOfType<TMP_InputField>().ToOption().Value;
            var email = texts.SingleOrDefault(t => t.name == "EmailInput")?.text;
            var user = texts.SingleOrDefault(t => t.name == "UsernameInput")?.text;
            var password = texts.Single(t => t.name == "PasswordInput").text;

            return new LoginDto(buttonParams.Value, email ?? user, password);
        }   

        protected override IResponseProcessStrategy<UserIdentityDtoWithToken> GetResponseProcessStrategy(SingleActionParam<LoginType> buttonParams)
        {
            return this;
        }

        public void OnSuccess(UserIdentityDtoWithToken responseDto)
        {
            PlayerPrefs.SetString(AuthConstants.GoogleTokenKey, responseDto.Token);
            PlayerPrefs.SetInt(AuthConstants.GoogleTokenExpirationKey, int.MaxValue);
            PlayerPrefs.SetString(AuthConstants.GoogleTokenSaveDateKey,
                _timeProvider.Now.ToBinary().ToString());
            PlayerPrefs.Save();
            Scene.MainMenuScreen.Load();
        }

        public void OnFail(ErrorResponse error)
        {
            error.DisplayToUi();
        }
    }
}