#nullable enable
using Common;
using Generated;
using UI.Common.Button;
using UnityEngine;

namespace UI.Login.Button
{
    public class LoginGoogleButtonControl : ButtonControl<NoButtonParams>
    {
        protected override void OnClickedTypesafe(NoButtonParams param)
        {
            var googleClientId = "565314247848-mjtqs7ioqn0002ti9hpne8icgd8dnql8.apps.googleusercontent.com";
            var baseUrl = "https://localhost:44366";

            var url =
                "https://accounts.google.com/o/oauth2/v2/auth?" +
                $"client_id={googleClientId}&" +
                $"redirect_uri={baseUrl}/google-login&" +
                "response_type=code&" +
                "scope=email&" +
                "access_type=offline&" +
                "include_granted_scopes=true";
            Application.OpenURL(url);
            Scenes.OAuthLoginScreen.Load();
        }
    }
}