#nullable enable
using Common;
using Generated;
using UI.Common.Button;

namespace UI.IntroScreen.Button
{
    public class LoginButtonControl : ButtonControl<NoButtonParams>
    {
        protected override void OnClickedTypesafe(NoButtonParams param)
        {
            Scenes.LoginScreen.Load();
        }
    }
}