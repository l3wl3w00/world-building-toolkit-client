#nullable enable
using Generated;
using UI.Common.Button.SceneChange;

namespace UI.IntroScreen.Command
{
    public class LoginCommand : SceneChangeCommand
    {

        protected override Scenes Scene => 
            Scenes.LoginScreen;
    }
}