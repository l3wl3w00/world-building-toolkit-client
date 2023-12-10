#nullable enable
using Common.ButtonBase;
using Common.Generated;
using Common.Triggers;
using Common.Utils;
using UnityEngine;

namespace UI.Login.Command
{
    public class LoginGoogleCommand : ActionListener
    {
        public override void OnTriggered(NoActionParam param)
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
            Scene.OAuthLoginScreen.Load();
        }
    }
}